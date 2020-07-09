let mongoClient = require('mongodb').MongoClient;
const db = {
    connectionString: process.env.DB_RO_CONNECTION_STRING,
    name: process.env.DB_NAME,
    container: process.env.DB_CONTAINER
};


module.exports = async function (context, req) {

    const client = await mongoClient.connect(db.connectionString)
        .catch(err => {
            context.log(`failed to connect to db: ${err.message}`);
            context.res = {
                status: 500,
                body: {
                    text: err.message
                }
            };
        })
    ;

    if( ! client ) {
        return;
    }


    try {
        let res = await client
            .db(db.name)
            .collection(db.container)
            .findOne( { id: context.bindingData.Id } )
        ;

        context.log(`db record found for id: ${context.bindingData.id}`);
        context.res = {
            status: 202,
            body: {
                text: res ? res.upVotes : -99
            }
        };
    }
    catch(err) {
        context.log(`db record found for id: ${context.bindingData.id}: error: ${err}`);
        context.res = {
            status: 500,
            body: {
                text: err
            }
        };
    }
};
