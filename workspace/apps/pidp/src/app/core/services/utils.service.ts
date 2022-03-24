import { DOCUMENT } from '@angular/common';
import { Inject, Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';

import { WINDOW } from '@bcgov/shared/utils';

export type SortWeight = -1 | 0 | 1;

@Injectable({
  providedIn: 'root',
})
export class UtilsService {
  public constructor(
    @Inject(WINDOW) private window: Window,
    @Inject(DOCUMENT) private document: Document
  ) {}

  /**
   * @description
   * Scroll to the top of the content container.
   *
   * Scrolling the content to a position has to be performed
   * on the container with the scrollbar. The default container
   * is considered to be the window.
   *
   * For example, when using Angular Material's SideNav
   * <mat-sidenav-content></mat-sidenav-content> will
   * be that container that needs to be scrolled using
   * the `.mat-sidenav-content` selector.
   *
   * @example utilsService('.mat-sidenav-content');
   */
  public scrollTop(containerQuerySelector?: string): void {
    const contentContainer = containerQuerySelector
      ? this.document.querySelector(containerQuerySelector) ?? this.window
      : this.window;
    contentContainer.scroll({ top: 0, left: 0, behavior: 'smooth' });
  }

  /**
   * @description
   * Scroll to an anchor within the content container.
   */
  public scrollToAnchor(elementId: string | null): void {
    if (!elementId) {
      return;
    }

    this.document
      .querySelector(`#${elementId}`)
      ?.scrollIntoView({ behavior: 'smooth' });
  }

  /**
   * @description
   * Generic sorting of a JSON object by key.
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public sortByKey<T extends { [key: string]: any }>(
    key: keyof T
  ): (a: T, b: T) => SortWeight {
    return (a: T, b: T): SortWeight => this.sort<T>(a[key], b[key]);
  }

  /**
   * @description
   * Generic sorting of a JSON object by direction.
   */
  public sortByDirection<T>(
    a: T,
    b: T,
    direction: SortDirection = 'asc',
    withTrailingNull: boolean = true
  ): SortWeight {
    let result: SortWeight;

    if (a === null && withTrailingNull) {
      result = -1;
    } else if (b === null && withTrailingNull) {
      result = 1;
    } else {
      result = this.sort(a, b);
    }

    if (direction === 'desc') {
      result *= -1;
    }

    return result as SortWeight;
  }

  /**
   * @description
   * Generic sorting of a JSON object by key.
   */
  public sort<T>(a: T, b: T): SortWeight {
    return a > b ? 1 : a < b ? -1 : 0;
  }

  /**
   * @description
   * Download a document.
   */
  public downloadDocument(file: Blob, filename: string): void {
    // No downloads for IE or Edge browsers prior to
    // Chromium-based Edge
    if (this.isIEOrPreChromiumEdge()) {
      return;
    }

    const data = URL.createObjectURL(file);
    const link = document.createElement('a');
    link.href = data;
    link.download = filename;
    link.target = '_blank';
    link.click();
    setTimeout(() => {
      URL.revokeObjectURL(data);
      link.remove();
    }, 100);
  }

  /**
   * @description
   * Download a PDF.
   */
  public downloadPdf(file: string | Blob, filename: string): void {
    if (typeof file === 'string') {
      file = this.base64ToBlob(file);
    }

    this.downloadDocument(file, filename);
  }

  /**
   * @description
   * Checks if the browser is Internet Explorer, or pre-Chromium
   * Edge.
   *
   * The reversed attribute of ordered lists is not supported in IE or
   * pre-Chromium Edge, but has been supported in all other browsers
   * forevers!!!
   * @see https://caniuse.com/?search=reversed
   */
  public isIEOrPreChromiumEdge(): boolean {
    return !('reversed' in document.createElement('ol'));
  }

  /**
   * @description
   * Conversion of Base64 encoded document to a Blob.
   */
  private base64ToBlob(base64: string, type: string = 'application/pdf'): Blob {
    const decoded = window.atob(base64.replace(/\s/g, ''));
    const len = decoded.length;
    const buffer = new ArrayBuffer(len);
    const data = new Uint8Array(buffer);

    for (let i = 0; i < len; i++) {
      data[i] = decoded.charCodeAt(i);
    }

    return new Blob([data], { type });
  }
}
