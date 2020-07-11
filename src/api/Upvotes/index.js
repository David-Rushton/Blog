const mongo = require('mongodb');
const mongoClient = require('mongodb').MongoClient;
const blogDbConfig = (() => {

    let isProduction = ('BLOG_ENVIRONMENT' in process.env && process.env.BLOG_ENVIRONMENT == 'PRODUCTION');
    let containerName = process.env.DB_CONTAINER;
    let containerNameDev = `${process.env.DB_CONTAINER}-dev`;

    return {
        dbConnectionString: process.env.DB_RW_CONNECTION_STRING,
        dbName: process.env.DB_NAME,
        dbContainer: isProduction ? containerName : containerNameDev
    };
})();


/**
 * Exception method for reporting encountered issues
 * @param {int} status -
 * @param {string} message -
 */
function upVoteException(status, message) {
    this.status = status;
    this.message = message;

    this.toString = function() {
        `status: ${status} message: ${message}`;
    };
}


/**
 * Read and write to the db
 */
class blogDb {

    constructor() {
        this.client;
        this.container;
    }


    /**
     * Connects to the blog db
     * @param {string} connectionString - Cosmos db connection string
     * @param {string} dbName - Cosmos db database name
     * @param {string} dbContainer - Comsmos db container name
     */
    async connect({dbConnectionString, dbName, dbContainer}) {
        this.client = await mongoClient
            .connect(
                dbConnectionString,
                { useNewUrlParser: true, useUnifiedTopology: true }
            )
        ;

        this.container = this.client
            .db(dbName)
            .collection(dbContainer)
        ;
    }

    /**
     * Returns an article from the db by id.
     * If the id does not exist null is returned.
     * @param {string} id
     */
    async getArticle(id) {
        return await this.container
            .findOne({ _id: id })
        ;
    }

    /**
     * Increment the number of upVotes for an article by 1.
     * @param {string} id
     */
    async incrementUpVotes(id) {
        return await this.container
            .updateOne({ _id: id }, { $inc: { upVotes: 1 } })
        ;
    }


    /**
     * Closes our connection to the blog db.
     */
    disconnect() {
        this.client.close();
    }
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

    const db = new blogDb();

    try {

        const method = req.method.toUpperCase();
        const id = context.bindingData.id;
        let article;

        await db.connect(blogDbConfig);


        if( ! ['GET', 'POST'].includes(method) )
            throw new upVoteException(`request method not supported: ${method}`);

        if (method == 'POST')
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
