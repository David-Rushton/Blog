const mongo = require('mongodb');
const mongoClient = require('mongodb').MongoClient;


/**
 * Returns blogdb config, including the connection string
 */
const getDatabaseConfig = () => {

    let isProduction = ('BLOG_ENVIRONMENT' in process.env && process.env.BLOG_ENVIRONMENT == 'PRODUCTION');
    let containerName = process.env.DB_CONTAINER;
    let containerNameDev = `${process.env.DB_CONTAINER}-dev`;

    return {
        connectionString: process.env.DB_RW_CONNECTION_STRING,
        name: process.env.DB_NAME,
        container: isProduction ? containerName : containerNameDev
    };
};


module.exports = async function(context, req) {

    const config = getDatabaseConfig();
    const client = await mongoClient.connect(config.connectionString, { useNewUrlParser: true, useUnifiedTopology: true });

    try {

        const method = req.method;
        const id = context.bindingData.id;
        let res = await client
            .db(config.name)
            .collection(config.container)
            .findOne({ _id: id })
        ;

        if( ! res ) {
            context.res = {
                status: 404,
                body: {
                    message: 'article not found'
                }
            };

            return;
        }

        context.res = {
            status: 200,
            body: {
                message: 'article retrieved',
                article: res
            }
        };
    }
    catch(err) {

        context.res = {
            status: 500,
            body: {
                message: err.message
            }
        };
    }
    finally {

        client.close();
    }
};
