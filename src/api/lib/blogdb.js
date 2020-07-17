'use strict';

const mongo = require('mongodb');
const mongoClient = require('mongodb').MongoClient;


/**
 * Read and write to the db
 */
module.exports = class BlogDb {

    constructor() {
        this.client;
        this.container;
    }


    /**
     * Connects to the blog db
     * @param {string} connectionString - Cosmos db connection string
     * @param {string} name - Cosmos db database name
     * @param {string} container - Comsmos db container name
     */
    async connect({connectionString, name, container}) {
        this.client = await mongoClient
            .connect(
                connectionString,
                { useNewUrlParser: true, useUnifiedTopology: true }
            )
        ;

        this.container = this.client
            .db(name)
            .collection(container)
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
     * Adds a comment to the article
     * @param {string} id
     * @param {string} commentId
     * @param {string} parentCommentId
     * @param {string} username
     * @param {string} comment
     */
    async addComment(id, {commentId, parentCommentId, username, comment}) {
        return await this.container
            .updateOne(
                { _id: id },
                { $push: {
                        comments: {
                            commentId: commentId,
                            parentCommentId: parentCommentId,
                            username: username,
                            comment: comment,
                            posted: new Date()
                        }
                    }
                }
            )
        ;
    }


    /**
     * Closes our connection to the blog db.
     */
    disconnect() {
        this.client.close();
    }
}
