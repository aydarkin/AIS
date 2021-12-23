import Data from "./Data";
import Cookie from "./Cookie";

export default class Profile {
  static _model: any;

  static get model(): Promise<any> {
    if (Profile._model) {
      return Promise.resolve(Profile._model);
    }

    const id = Cookie.getCookie("userId");
    return Data.getQuery("person/" + id).then((person) => {
      Profile._model = person;
      return Profile._model;
    });
  }

  static clear() {
    Profile._model = undefined;
  }

  static getAge(dateISO: string, customYear?: number): string {
    const yearBirth = new Date(dateISO).getFullYear();
    const year = customYear ?? new Date().getFullYear();

    return (year - yearBirth).toString();
  }

  static ageToStr(age: string): string {
    let txt;
    let count = Number(age) % 100;
    if (count >= 5 && count <= 20) {
      txt = "лет";
    } else {
      count = count % 10;
      if (count == 1) {
        txt = "год";
      } else if (count >= 2 && count <= 4) {
        txt = "года";
      } else {
        txt = "лет";
      }
    }
    return age + " " + txt;
  }
}
