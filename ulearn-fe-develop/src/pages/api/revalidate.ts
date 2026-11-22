import { NextApiRequest, NextApiResponse } from 'next';
import routes from 'routes/routes';

const REVALIDATION_PATHS = {
    HOME: routes.home.path,
    TUTOR: routes.tutorLandingPage.path,
    COMPANY: routes.companyLandingPage.path,
    TERMS_OF_USE: routes.termsOfUse.path,
    PRIVACY_POLICE: routes.privacyPolicy.path
};

export default async function revalidateHandler(req: NextApiRequest, res: NextApiResponse) {
    const { secret } = req.query;

    if (secret !== process.env.REVALIDATION_TOKEN) {
        return res.status(401).json({ message: 'Invalid token' });
    }

    try {
        await Promise.all([
            // res.revalidate(REVALIDATION_PATHS.HOME),
            // res.revalidate(REVALIDATION_PATHS.TUTOR),
            // res.revalidate(REVALIDATION_PATHS.COMPANY),
            res.revalidate(REVALIDATION_PATHS.TERMS_OF_USE),
            res.revalidate(REVALIDATION_PATHS.PRIVACY_POLICE)
        ]);

        return res.json({ revalidated: true });
    } catch (err) {
        console.error('Error revalidating paths: ', err);
        return res.status(500).send('Error revalidating paths');
    }
}