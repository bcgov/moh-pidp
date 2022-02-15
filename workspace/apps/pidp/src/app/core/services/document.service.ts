import { Inject, Injectable } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

@Injectable({
  providedIn: 'root',
})
export class DocumentService {
  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {}

  public getAuthCollectionNotice(): string {
    return `
      The Provider Identity Portal collects personal information for the purposes of verification and access
      to participating health systems. This is collected by the Ministry of Health under sections 26(c) and
      27(1)(b) of the Freedom of Information and Protection of Privacy Act. Should you have any questions 
      about the collection of this personal information, contact
      <a href="mailto:${this.config.emails.providerIdentitySupport}">${this.config.emails.providerIdentitySupport}</a>.
    `;
  }

  public getSAeFormsCollectionNotice(): string {
    // TODO add pipe that creates an anchor when an email address is found and remove markup from text
    return `
      The personal information you provide to enrol for access to the Special Authority eForms application
      is collected by the British Columbia Ministry of Health under the authority of s. 26(a) and 26(c) of
      the Freedom of Information and Protection of Privacy Act (FOIPPA) and s. 22(1)(b) of the Pharmaceutical
      Services Act for the purpose of managing your access to, and use of, the Special Authority eForms
      application. If you have any questions about the collection or use of this information, contact
      <a href="mailto:${this.config.emails.specialAuthoritySupport}">${this.config.emails.specialAuthoritySupport}</a>.
    `;
  }
}
