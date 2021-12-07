import { Injectable } from '@angular/core';

function getWindow(): Window {
  return window;
}

@Injectable({
  providedIn: 'root',
})
export class WindowRefService {
  /**
   * @description
   * Get a reference to the native window object.
   */
  public get nativeWindow(): Window {
    return getWindow();
  }
}
