import { Type } from '@angular/core';

import { IDialogContent } from './dialog-content.model';

export interface DialogOptions {
  icon?: string;
  iconType?: 'outlined' | 'round' | 'sharp';
  imageSrc?: string; // Alternative to an icon
  imageType?: 'icon' | 'banner';
  imageSizeFull?: boolean;
  title?: string;
  titlePosition?: 'center' | 'left' | 'right';
  titleMarginTop?: boolean;
  bottomBorder?: boolean;
  message?: string;
  bodyTextPosition?: 'center' | 'left' | 'right';
  actionType?: 'primary' | 'accent' | 'warn';
  actionTypePosition?: 'center' | 'left' | 'right';
  actionText?: string;
  actionHide?: boolean;
  closeButton?: boolean;
  actionLink?: {
    href: string;
    text: string;
  };
  cancelText?: string;
  cancelHide?: boolean;
  component?: Type<IDialogContent>;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  data?: { [key: string]: any };
  /** overload the dialog width, if undefined the default value will be used */
  width?: string;
  /** overload the dialog height, if undefined the default value will be used */
  height?: string;
  class?: string;
}
