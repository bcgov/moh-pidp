import { serverErrorInterceptorProvider } from './server-error.interceptor';
import { unauthorizedInterceptorProvider } from './unauthorized.interceptor';
import { unavailableInterceptorProvider } from './unavailable.interceptor';

/**
 * @description
 * HTTP interceptor providers in the expected
 * outside-in order.
 */
export const httpInterceptorProviders = [
  unavailableInterceptorProvider,
  unauthorizedInterceptorProvider,
  serverErrorInterceptorProvider,
];
