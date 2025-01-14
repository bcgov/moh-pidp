import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';

import { IsHighAssurancePipe } from './is-high-assurance.pipe';

describe('IsHighAssurancePipe', () => {
  let pipe: IsHighAssurancePipe;
  beforeEach(() => (pipe = new IsHighAssurancePipe()));

  it('create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('returns true when BCSC', () => {
    expect(pipe.transform(IdentityProvider.BCSC)).toEqual(true);
  });

  it('returns true when BCProvider', () => {
    expect(pipe.transform(IdentityProvider.BC_PROVIDER)).toEqual(true);
  });

  it('returns false when null', () => {
    expect(pipe.transform(null)).toEqual(false);
  });

  it('returns false when idir', () => {
    expect(pipe.transform(IdentityProvider.IDIR)).toEqual(false);
  });

  it('returns false when phsa', () => {
    expect(pipe.transform(IdentityProvider.PHSA)).toEqual(false);
  });
});
