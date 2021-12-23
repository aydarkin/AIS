// @ts-nocheck
import Profile from '@/utils/Profile'

describe('Профиль', () => {
  describe('Возраст', () => {
    const inputAge = ['1', '2', '12', '20'];
    const expectedAge = ['год', 'года', 'лет', 'лет'];

    for (let i = 0; i < inputAge.length; i++) {
      it('Текст возраста ' + inputAge[i], () => {
        expect(Profile.ageToStr(inputAge[i])).toMatch(inputAge[i] + ' ' + expectedAge[i]);
      });
    }  

    const testDate = new Date(2021, 11, 28);
    const input = ['1998-02-02', '1990-02-02', '2004-06-02', '2001-12-12'];
    const expected = ['23', '31', '17', '20'];

    for (let i = 0; i < inputAge.length; i++) {
      it(`Количество лет между ${input[i]} и 2021-11-28`, () => {
        expect(Profile.getAge(input[i], testDate.getFullYear())).toMatch(expected[i]);
      });
    } 
    
  });
})
