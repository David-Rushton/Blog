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


// Api entry point
module.exports = async function(context, req) {

    const db = new BlogDb();

    try {

        const method = req.method.toUpperCase();
        const id = context.bindingData.id;
        let response;

        await db.connect(config.db);
        response = await db.incrementUpVotes(id);

        if( ! response || response.modifiedCount == 0 )
            throw new upVoteException(404, `cannot find article: ${id}`);


        context.res = {
            status: 200,
            body: {
                message: `upvoted article: ${id}`
            }
        };
    }
    catch(e) {

        context.res = {
            status: (e instanceof upVoteException) ? e.status : 500,
            body: {
                message: e.message
            }
        };
    }
    finally {

        db.disconnect();
    }
};
