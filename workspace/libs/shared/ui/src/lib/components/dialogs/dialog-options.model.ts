export interface DialogOptions {
  icon?: string;
  iconType?: 'outlined' | 'round' | 'sharp';
  imageSrc?: string; // Alternative to an icon
  title?: string;
  message?: string;
  actionType?: 'primary' | 'accent' | 'warn';
  actionText?: string;
  actionHide?: boolean;
  actionLink?: {
    href: string;
    text: string;
  };
  cancelText?: string;
  cancelHide?: boolean;
  component?: any;
  data?: { [key: string]: any };
}
