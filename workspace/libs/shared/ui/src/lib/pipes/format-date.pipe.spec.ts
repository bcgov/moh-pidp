import { FormatDatePipe } from './format-date.pipe';

describe('FormatDatePipe', () => {
  let pipe: FormatDatePipe;
  beforeEach(() => (pipe = new FormatDatePipe()));

  it('create an instance', () => expect(pipe).toBeTruthy());

  it('should format the date to the default date format', () => {
    const value = '1977-09-22T14:30:00';
    const result = pipe.transform(value);
    expect(result).toBe('22 Sep 1977');
  });

  it('should format the date to a requested format', () => {
    const value = '1977-09-22T14:30:00';
    const format = 'MMMM dd, yyyy HH:mm a';
    const result = pipe.transform(value, format);
    expect(result).toBe('September 22, 1977 14:30 PM');
  });

  it('should format the date and hours to a requested hours format', () => {
    const value = '1984-02-15T15:32:00';
    const format = 'h:mm a';
    const result = pipe.transform(value, format);
    expect(result).toBe('3:32 PM');
  });

  it('should not fail when passed a null', () => {
    const value = null;
    const result = pipe.transform(value);
    expect(result).toBeNull();
  });
});
