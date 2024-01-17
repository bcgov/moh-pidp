/* eslint-disable @typescript-eslint/no-explicit-any */
import { ObjectUtils } from './object-utils.class';

describe('ObjectUtils', () => {
  const util = ObjectUtils;

  describe('keyMapping', () => {
    let object: { id: number; username: string; name: string };

    beforeEach(() => {
      object = {
        id: 1,
        username: 'giant@chicken.com',
        name: 'Foghorn Leghorn',
      };
    });

    it('should map object keys that exist within the source object', () => {
      util.keyMapping(object, { username: 'email', name: 'fullName' });

      expect(object).toEqual(
        expect.objectContaining({
          id: 1,
          email: 'giant@chicken.com',
          fullName: 'Foghorn Leghorn',
        }),
      );
      expect(object).not.toEqual(
        expect.objectContaining({
          username: 'giant@chicken.com',
          name: 'Foghorn Leghorn',
        }),
      );
    });

    it('should not map object keys that do not exist within the source object', () => {
      util.keyMapping(object, {
        username: 'email',
        name: 'fullName',
        notMapped: 'true',
      });

      expect(object).toEqual(
        expect.objectContaining({
          id: 1,
          email: 'giant@chicken.com',
          fullName: 'Foghorn Leghorn',
        }),
      );
      expect(object as any).not.toEqual(
        expect.objectContaining({
          notMapped: 'true',
        }),
      );
    });

    it('should not fail when passed a null source object', () => {
      util.keyMapping(null as any, { username: 'email', name: 'fullName' });

      expect(object).toEqual(
        expect.objectContaining({
          id: 1,
          username: 'giant@chicken.com',
          name: 'Foghorn Leghorn',
        }),
      );
    });

    it('should not fail when passed a null mapping', () => {
      util.keyMapping(object, null as any);

      expect(object).toEqual(
        expect.objectContaining({
          id: 1,
          username: 'giant@chicken.com',
          name: 'Foghorn Leghorn',
        }),
      );
    });
  });

  describe('mergeInto', () => {
    let object: { id: number; username: string; name: string };

    beforeEach(() => {
      object = {
        id: 1,
        username: 'giant@chicken.com',
        name: 'Foghorn Leghorn',
      };
    });

    it('should merge a key/value pair from the source into the output object', () => {
      const result = util.mergeInto('name', object, {
        quote: "Now who's, I say who's responsible for this",
      });

      expect(result).toEqual(
        expect.objectContaining({
          name: 'Foghorn Leghorn',
          quote: "Now who's, I say who's responsible for this",
        }),
      );
    });

    it('should note merge a key/value pair from the source into the output object when it does not exist', () => {
      const result = util.mergeInto('doesNotExist', object, {
        quote: "Now who's, I say who's responsible for this",
      });

      expect(Object.keys(result)).not.toContain('doesNotExist');
    });

    it('should not fail when passed a null key', () => {
      const merge = { quote: "Now who's, I say who's responsible for this" };
      const result = util.mergeInto(null as any, object, merge);

      expect(result).toEqual(merge);
    });

    it('should not fail when passed a null source object', () => {
      const merge = { quote: "Now who's, I say who's responsible for this" };
      const result = util.mergeInto('name', null as any, merge);

      expect(result).toEqual(merge);
    });

    it('should not fail when passed a null merge object', () => {
      const result = util.mergeInto('name', object, null as any);

      expect(result).toBeNull();
    });
  });
});
