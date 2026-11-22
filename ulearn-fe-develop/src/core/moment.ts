import moment from 'moment';
import 'moment/locale/uk';
import 'moment/locale/ru';
import 'moment/locale/cs';
import 'moment/locale/en-gb';
import 'moment/locale/sr';

export const setLanguage = (languageCode: string) => {
    moment.updateLocale(languageCode, { week: { dow: 1 } });
};