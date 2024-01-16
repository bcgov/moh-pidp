import { InjectionToken } from '@angular/core';

import { StringUtils } from '@bcgov/shared/utils';

import { DialogDefaultOptions } from './dialog-default-options.model';

export const defaultDialogOptions: DialogDefaultOptions = {
  unsaved: () => ({
    icon: 'error',
    title: 'Are you sure you want to leave?',
    message: `
    You have unsaved changes. Are you sure you want to leave this page?
    Unsaved changes will be lost.
    `,
    actionType: 'warn',
    actionText: 'Leave this page',
    cancelText: 'Stay on this page',
  }),
  delete: (modelName: string, supplementaryMessage = '') => {
    const capitalized = StringUtils.capitalize(modelName);
    return {
      title: `Delete ${capitalized}`,
      message: `Are you sure you want to delete this ${modelName.toLowerCase()}? ${supplementaryMessage}`,
      actionType: 'warn',
      actionText: `Delete ${capitalized}`,
    };
  },
};

export const DIALOG_DEFAULT_OPTION = new InjectionToken(
  'DIALOG_DEFAULT_OPTION',
  {
    providedIn: 'root',
    factory: (): DialogDefaultOptions => defaultDialogOptions,
  },
);
