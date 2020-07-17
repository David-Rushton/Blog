'use strict';

const isProduction = ('BLOG_ENVIRONMENT' in process.env && process.env.BLOG_ENVIRONMENT == 'PRODUCTION');
const containerName = process.env.DB_CONTAINER;
const containerNameDev = `${process.env.DB_CONTAINER}-dev`;


module.exports = {
    isProduction: isProduction,
    db: {
        connectionString: process.env.DB_RW_CONNECTION_STRING,
        name: process.env.DB_NAME,
        container: isProduction ? containerName : containerNameDev
    }
};
