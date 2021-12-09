import Cookie from './Cookie';

export default class Data {
    static domain = 'http://localhost:5002/api/';
        
    static async query(relativeURL: string, method: string, contentType?: string, body?: any, waitResult: boolean = true) {
        let params = {
            method,
            headers: {},
            //credentials: 'include',
        } as any;
        if (contentType) {
            params = {
                ...params,
                headers: {
                    'Content-Type': contentType
                }
            }
        }

        const token = Cookie.getCookie('token');
        params.headers.Authorization = "Bearer " + (token || '');

        if(body) {
            params = {...params, body: body }
        }

        const response = await fetch(this.domain + relativeURL, params);
        if(response.status >= 200 && response.status < 300) {
            if(waitResult) {
                return await response.json();
            }
            return;
        }
        throw new Error('[DataModule]' + response.statusText);
    }

    static async getQuery(relativeURL: string, params?: Record<string, string>, waitResult: boolean = true) {
        let url = relativeURL;
        if(params) {
            url = `${relativeURL}?${new URLSearchParams(params)}`
        }
        return await this.query(url, 'GET', undefined, null, waitResult)
    }

    static async postQuery(relativeURL: string, params: any, contentType = 'application/x-www-form-urlencoded', waitResult = true) {
        let body;
        switch (contentType) {
            case 'application/x-www-form-urlencoded':
                body = new URLSearchParams(params).toString();
                break;           
            case 'application/json':
            case 'application/json;charset=utf-8': 
                body = JSON.stringify(params);  
                break;
            default:
                body = params;
                break;
        }
        return await this.query(relativeURL, 'POST',  contentType, body, waitResult);
    }

    static async jsonQuery(relativeURL: string, params: any, waitResult = true) {
        return await this.postQuery(relativeURL, params, 'application/json', waitResult);
    }

    static async pngQuery(relativeURL: string, file: any, waitResult = true) {
        return await this.postQuery(relativeURL, file, 'image/png', waitResult);
    }
    
}