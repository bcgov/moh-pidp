import { Type } from '@angular/core';

export type ComponentType<T = unknown> = Type<T>;
export interface ProfileStatusAlert {
  heading: string;
  content: ComponentType | string;
}
