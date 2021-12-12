import Data from './Data';
import Cookie from './Cookie';

export default class Profile {
    static _model: any;

    static get model(): Promise<any> {   
        if (Profile._model) {
            return Promise.resolve(Profile._model);
        }

        const id = Cookie.getCookie('userId');
        return Data.getQuery('person/' + id).then((person) => {
            Profile._model = person;
            return Profile._model;
        });
    }

    static clear() {
        Profile._model = undefined;
    }
}