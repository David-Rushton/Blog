'use strict';

const BlogDb = require('../lib/blogdb');
const config = require('../lib/config');
const { v4: uuidv4 } = require('uuid');


/**
 * Exception method for reporting issues
 * @param {int} status - http status code
 * @param {string} message - exception message
 */
function commentsException(status, message) {
    this.status = status;
    this.message = message;

    this.toString = function() {
        `status: ${status} message: ${message}`;
    };
}


/**
 * Returns the current usersname, or an empty string if no one logged in
 */
function getUsername() {

    // required auth header is not present in local dev builds
    // TODO: proxy staging or prod auth into local dev env
    if( ! config.isProduction )
        return 'test-user';

    if( ! req.headers.includes('x-ms-client-principal') )
        throw new commentsException(401, 'user not authorised');


    const adminUserPrefix = '⭐';
    const header = req.headers["x-ms-client-principal"];
    const encoded = Buffer.from(header, "base64");
    const decoded = encoded.toString("ascii");
    const user = JSON.parse(decoded);


    if(user.userRoles.includes('admin'))
        return `${adminUserPrefix}${user.userDetails}`;

    return user.userDetails;
}


/**
 * Ensures binding data contains all fields required to create a new comment
 * @param {object} data - context binding data object
 */
function getCommentDetailsOrThrowIfMissing(data) {

    let errors = [];

    if( ! data.parentCommentId )
        errors.push('parentCommentId');

    if( ! data.username )
        errors.push('username');

    if( ! data.comment )
        errors.push('comment');


    if(errors.length > 0)
        throw new commentsException(400, `required field(s) missing: ${errors.join(', ')}`);


    return {
        commentId: uuidv4(),
        parentCommentId: data.parentCommentId,
        username: data.username,
        comment: data.comment
    }
}


// Api entry point
module.exports = async function (context, req) {

    const db = new BlogDb();

    try {

        const method = req.method.toUpperCase();
        const id = context.bindingData.id;
        let article;

        await db.connect(config.db);


        if( ! ['GET', 'POST'].includes(method) )
            throw new commentsException(500, `request method not supported: ${method}`);

        if(method == 'POST') {
            const comment = getCommentDetailsOrThrowIfMissing(context.bindingData);
            article = await db.addComment(id, comment);
        }

        if(method == 'GET')
            article = await db.getArticle(id);

        if( ! article || article.modifiedCount == 0 )
            throw new commentsException(404, `cannot find article: ${id}`);


        context.res = {
            status: 200,
            body: {
                message: method == 'GET' ? `article comments found: ${id}` : `article comment added: ${id}`,
                article: article
            }
        };
    }
    catch(e) {

        if(e instanceof commentsException) {
            context.res = {
                status: e.status,
                body: {
                    message: e.message
                }
            };
        }
        else {
            context.rest = {
                status: 500,
                body: {
                    message: e.toString()
                }
            };
        }
    }
    finally {

        db.disconnect();
    }
};
