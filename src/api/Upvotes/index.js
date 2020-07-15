'use strict';

const BlogDb = require('../lib/blogdb');
const config = require('../lib/config');


/**
 * Exception method for reporting encountered issues
 * @param {int} status - http status code
 * @param {string} message - exception message
 */
function upVoteException(status, message) {
    this.status = status;
    this.message = message;

    this.toString = function() {
        `status: ${status} message: ${message}`;
    };
}


/**
 * Returns a failed request to the caller
 * @param {object} context - Http request context
 * @param {number} status - Http status code to return
 * @param {string} message - Error description
 */
const returnError = (context, status, message) => {
    context.res = {
        status: status,
        body: {
            message: message
        }
    };
}


// Api entry point
module.exports = async function(context, req) {

    const db = new BlogDb();

    try {

        const method = req.method.toUpperCase();
        const id = context.bindingData.id;
        let article;

        await db.connect(config.db);


        if( ! ['GET', 'POST'].includes(method) )
            throw new upVoteException(500, `request method not supported: ${method}`);

        if(method == 'POST')
            article = await db.incrementUpVotes(id);

        if(method == 'GET')
            article = await db.getArticle(id);

        if( ! article || article.modifiedCount == 0 )
            throw new upVoteException(404, `cannot find article: ${id}`);


        context.res = {
            status: 200,
            body: {
                message: `article found: ${id}`,
                article: article
            }
        };
    }
    catch(e) {

        if(e instanceof upVoteException) {
            context.res = {
                status: e.status,
                body: {
                    message: e.message
                }
            };
        }
        else {
            context.res = {
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
