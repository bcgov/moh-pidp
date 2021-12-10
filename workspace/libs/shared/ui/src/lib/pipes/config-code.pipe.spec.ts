import { ConfigCodePipe } from './config-code.pipe';

describe('ConfigCodePipe', () => {
  let pipe: ConfigCodePipe;
  beforeEach(() => (pipe = new ConfigCodePipe()));

  it('create an instance', () => expect(pipe).toBeTruthy());

  // TODO update tests to not have dependencies on services
  // it('should get a config name from a config code', () => {
  //   const country = configService.countries[0];
  //   const result = pipe.transform(country.code, 'countries');
  //   expect(result).toBe(country.name);
  // });

  // it('should not fail when passed a null', () => {
  //   const country = configService.countries[0];
  //   const result = pipe.transform(null, 'countries');
  //   expect(result).toBe('');
  // });
});
