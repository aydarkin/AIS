export default class Cookie {

    static getCookie(name: string): string | undefined {
        const results = document.cookie.match('(^|;) ?' + name + '=([^;]*)(;|$)');
        
        if (results)
            return (unescape(results[2]));
        else
            return undefined;
    }
}