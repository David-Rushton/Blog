let mongoClient = require('mongodb').MongoClient;
let objectId = require('mongodb').ObjectID;
const connectionString = process.env.DB_CONNECTION_STRING;


module.exports = async function (context, req) {
/*
    mongoClient.connect(connectionString, (err, client) => {

        if(err) {
            console.error(`failed to connection to cosmos: ${err.message}`);
            console.error(`cosmos connection string: ${connectionString}`);
            context.res = {
                body: {
                    message: "An error occurred",
                    connectionString: connectionString,
                    error: err.message
                }
            }
            return
        }

        let db = client.db('blogdb');
        let col = db.collection('articles');

        col.insertOne(
            {
                _id: "/blog.articles/making-code-pop.html",
                upVotes: 1
            }
        , (err, res) => {
            if(err) {
                console.error(`mongo err: ${err}`);
                console.log(res);
            }
        });


        client.close();
    });
*/

    mongoClient.connect(connectionString, (err, client) => {
        if(err) {
            console.error(err);
        }

        let db = client.db('blogdb');
        let cursor = db.collection('articles').find();
        cursor.each((err, doc) => {
            if(err) {
                console.error(err);
            }

            console.log(doc);
        });


        client.close();
    });




    context.res = {
        body: {
            text: "Hello from the API"
        }
    }
};
