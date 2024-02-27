import { ResolveFn } from '@angular/router';

export const accountLinkingResolver: ResolveFn<boolean> = (route, state) => {
  return true;
};
