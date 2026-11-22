/** @type {import('next-sitemap').IConfig} */
const ENV_URL = process.env.NEXT_PUBLIC_DOMAIN_URL;
const PROD_URL = 'https://www.yaspeak.com';
const ALTERNATIVE_REFS = ['en', 'ru', 'uk', 'sr'];

const getLocaleRegex = () => new RegExp(`\/(((${ALTERNATIVE_REFS.join('|')})\/)|((${ALTERNATIVE_REFS.join('|')})$))`);
const includesLocale = (url) => getLocaleRegex().test(url);
const getWithReplacedLocale = (path, newLocale) => {
  const relativePathWithLocale = path.replace(ENV_URL, '');
  const relativePath = relativePathWithLocale.replace(getLocaleRegex(), '');
  return [ENV_URL, newLocale, relativePath]
    .filter(x => x !== '')
    .filter(x => x !== '/')
    .join('/');
}


module.exports = {
  siteUrl: ENV_URL,
  exclude: [
    '/',
    '/sitemap/*',
    '/tutors/search',
    '/tutor/dashboard',
    '/student/dashboard',
    '/admin/dashboard',
    '/company/dashboard',
    '/auth/forgotten-password',
    '/auth/set-password',
    '/auth/reset-password',
    '/auth/google/company',
    '/auth/facebook/company',
    '/auth/seznam/company',
    '/auth/google/tutor',
    '/auth/facebook/tutor',
    '/auth/seznam/tutor',
    '/auth/google/student',
    '/auth/facebook/student',
    '/auth/seznam/student',
    '/auth/confirm',
    '/auth/code-verification',
    '/tutor/registration',
    '/company/registration',
    '/tutor/verification',
    '/tutor/settings',
    '/student/settings',
    '/company/settings',
    '/messages',
    '/payment/reservation/success',
    '/payment/money-refill/success',
    '/notifications',
    '/meeting/*'
  ],
  generateRobotsTxt: ENV_URL === PROD_URL,
  robotsTxtOptions: {
    additionalSitemaps: [
      '/sitemap/tutor-sitemap.xml',
      '/sitemap/subject-sitemap.xml',
      '/sitemap/home-sitemap.xml',
      '/sitemap/specialization-sitemap.xml',
    ].map(x => ENV_URL + x)
  },
  transform: async (config, path) => {
    return includesLocale(path) ? {
      loc: path,
      changefreq: config.changefreq,
      priority: config.priority,
      lastmod: config.autoLastmod ? new Date().toISOString() : undefined,
      alternateRefs: ALTERNATIVE_REFS.map(x => ({
        href: getWithReplacedLocale(path, x),
        hreflang: x,
        hrefIsAbsolute: true
      })),
    } : null
  }
}