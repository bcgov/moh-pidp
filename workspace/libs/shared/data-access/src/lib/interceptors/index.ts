import { Provider } from '@angular/core';

import { loadingInterceptorProvider } from './loading.interceptor';
import { serverErrorInterceptorProvider } from './server-error.interceptor';
import { unauthorizedInterceptorProvider } from './unauthorized.interceptor';
import { unavailableInterceptorProvider } from './unavailable.interceptor';

export { SHOW_LOADING_MESSAGE } from './loading.interceptor';

/**
 * @description
 * HTTP interceptor providers in the expected
 * outside-in order.
 */
export function provideHttpInterceptors(): Provider[] {
  return [
    loadingInterceptorProvider,
    unavailableInterceptorProvider,
    unauthorizedInterceptorProvider,
    serverErrorInterceptorProvider,
  ];
}
