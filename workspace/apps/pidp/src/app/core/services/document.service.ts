import { Inject, Injectable } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Injectable({
  providedIn: 'root',
})
export class DocumentService {
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    // TODO move this into a pipe and create the anchor dynamically
    private domSanitizer: DomSanitizer
  ) {}

  public getCollectionNotice(): SafeHtml {
    const collectionNotice = `
      The personal information you provide to enrol for access to the Special Authority e-Forms application
      is collected by the British Columbia Ministry of Health under the authority of s. 26(a) and 26(c) of
      the Freedom of Information and Protection of Privacy Act (FOIPPA) and s. 22(1)(b) of the Pharmaceutical
      Services Act for the purpose of managing your access to, and use of, the Special Authority e-Forms
      application. If you have any questions about the collection or use of this information, contact
      <a href="mailto:${this.config.emails.specialAuthoritySupport}">${this.config.emails.specialAuthoritySupport}</a>.
    `;

    return this.domSanitizer.bypassSecurityTrustHtml(collectionNotice);
  }
}
