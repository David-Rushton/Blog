'use strict';

const BlogDb = require('../lib/blogdb');
const config = require('../lib/config');


/**
 * Method for raising exceptions
 * @param {int} status - http status code
 * @param {status} message - exception message
 */
function articleException(status, message) {
    this.status = status;
    this.message = message;

    this.toString = function() {
        return `status: ${status} message: ${message}`;
    }
}


// Api entry point
module.exports = async function (context, req) {

    const db = new BlogDb();

    try {

        const id = context.bindingData.id;
        let article;

        await db.connect(config.db);
        article = await db.getArticle(id);

        if( ! article )
            throw new articleException(404, `cannot find article: ${id}`);


        context.res = {
            status: 200,
            body: {
                message: `article found: ${id}`,
                article: article
            }
        };
    }
    catch(e) {

        context.res = {
            status: (e instanceof articleException) ? e.status : 500,
            body: {
                message: e.message
            }
        };
    }
    finally {

        db.disconnect();
    }
}
