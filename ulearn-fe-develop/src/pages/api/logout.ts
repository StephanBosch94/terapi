import { cookieConstants } from 'core/constants/cookieConstants';
import { NextApiRequest, NextApiResponse } from 'next';


export default async function logoutHandler(req: NextApiRequest, res: NextApiResponse) {
    try {
        res.setHeader('Set-Cookie', [
            `${cookieConstants.userLoginCookie}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT`,
            `${cookieConstants.bearer}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT`
        ]);
        return res.json({ logout: true });
    } catch (err) {
        console.error(err);
        return res.status(500).send('Error deleting cookies');
    }
}