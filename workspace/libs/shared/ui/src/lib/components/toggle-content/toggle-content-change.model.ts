import { MatSlideToggleChange } from '@angular/material/slide-toggle';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface ToggleContentChange
  extends Pick<MatSlideToggleChange, 'checked'> {}
