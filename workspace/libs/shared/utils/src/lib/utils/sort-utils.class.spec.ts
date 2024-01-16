import { SortUtils } from './sort-utils.class';

describe('SortUtils', () => {
  const utils = SortUtils;

  describe('METHOD sort', () => {
    given('an array of numbers to be sorted in ascending order', () => {
      const array = [1, 7, 2, 6, 8, 5, 4, 9, 3];

      when('the array is sorted', () => {
        const arraySorted = array.sort(utils.sort);

        then('it should be in the expected order', () => {
          expect(arraySorted).toEqual([1, 2, 3, 4, 5, 6, 7, 8, 9]);
        });
      });
    });

    given('an array of characters to be sorted in ascending order', () => {
      const array = ['c', 'f', 'd', 'g', 'h', 'b', 'e', 'a'];

      when('the array is sorted', () => {
        const arraySorted = array.sort(utils.sort);

        then('it should be in the expected order', () => {
          expect(arraySorted).toEqual(['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h']);
        });
      });
    });
  });

  describe('METHOD sortByKey', () => {
    given('an exceptional team of developers and friends', () => {
      const theOriginalPrimeTeam = [
        { name: 'Jason' },
        { name: 'Martin' },
        { name: 'James' },
        { name: 'Anais' },
        { name: 'Nathan' },
      ];

      when('the team is sorted for morning standup', () => {
        const theOriginalPrimeTeamAtStandup = theOriginalPrimeTeam.sort(
          utils.sortByKey('name'),
        );

        then('it should be in ascending order based on name', () => {
          expect(theOriginalPrimeTeamAtStandup).toEqual([
            { name: 'Anais' },
            { name: 'James' },
            { name: 'Jason' },
            { name: 'Martin' },
            { name: 'Nathan' },
          ]);
        });
      });
    });
  });
});
