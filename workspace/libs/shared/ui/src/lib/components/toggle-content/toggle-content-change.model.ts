import { MatSlideToggleChange } from '@angular/material/slide-toggle';

export interface ToggleContentChange
  extends Pick<MatSlideToggleChange, 'checked'> {}
