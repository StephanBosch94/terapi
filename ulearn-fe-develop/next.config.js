module.exports = {
    webpack(config) {
        config.module.rules.push({
            test: /\.svg$/,
            use: ["@svgr/webpack"]
        });

        return config;
    },
    i18n: {
        locales: ['default', 'en', 'ru', 'uk', 'sr'],
        // This is the default locale you want to be used when visiting
        // a non-locale prefixed path e.g. `/hello`
        defaultLocale: 'default',
        // This is a list of locale domains and the default locale they
        // should handle (these are only required when setting up domain routing)
        // Note: subdomains must be included in the domain value to be matched e.g. "fr.example.com".
        domains: []
    },
    images: {
        domains: [
            'localhost', 
            'int.yaspeak.com', 
            'yaspeak.com', 
            'www.yaspeak.com', 
            'beapi.int.yaspeak.com', 
            'prod.beapi.yaspeak.com',
            'api.yaspeak.lerybuilds.com'
        ]
    },
};
