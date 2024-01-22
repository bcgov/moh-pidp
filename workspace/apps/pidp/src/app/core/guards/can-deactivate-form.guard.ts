import { CanDeactivateFn } from '@angular/router';

import { IFormPage } from '../classes/abstract-form-page.class';

export const canDeactivateFormGuard: CanDeactivateFn<IFormPage> = (
  component,
) => {
  // Pass control to the component to determine if deactivation
  // can occur, otherwise, allow route request to occur
  return component.canDeactivate ? component.canDeactivate() : true;
};
