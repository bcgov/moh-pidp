import { ConfigMap } from './config-map.model';

export type environmentName = 'prod' | 'test' | 'dev' | 'local';

export interface AppEnvironment extends ConfigMap {
  // Only indicates that Angular has been built
  // using --configuration=production
  production: boolean;
}
