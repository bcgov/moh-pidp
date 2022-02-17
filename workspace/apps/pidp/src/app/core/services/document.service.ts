import { Inject, Injectable } from '@angular/core';

import { APP_CONFIG, AppConfig } from '@app/app.config';

export enum DocumentType {
  PIDP_COLLECTION_NOTICE = 'pidp-collection-notice',
  SA_EFORMS_COLLECTION_NOTICE = 'sa-eforms-collection-notice',
}

export interface IDocumentMetaData {
  type: DocumentType;
  title: string;
}

export interface IDocument extends IDocumentMetaData {
  content: string;
}

@Injectable({
  providedIn: 'root',
})
export class DocumentService {
  private documents: IDocumentMetaData[];

  public constructor(@Inject(APP_CONFIG) private config: AppConfig) {
    this.documents = [
      {
        type: DocumentType.PIDP_COLLECTION_NOTICE,
        title: 'PIdP Collection Notice',
      },
      {
        type: DocumentType.SA_EFORMS_COLLECTION_NOTICE,
        title: 'SA eForms Collection Notice',
      },
    ];
  }

  public getDocuments(): IDocumentMetaData[] {
    return this.documents;
  }

  public getDocumentByType(documentType: DocumentType): IDocument {
    switch (documentType) {
      case DocumentType.PIDP_COLLECTION_NOTICE:
        return {
          ...this.getDocumentMetaData(documentType),
          content: this.getPIdPCollectionNotice(),
        };
      case DocumentType.SA_EFORMS_COLLECTION_NOTICE:
        return {
          ...this.getDocumentMetaData(documentType),
          content: this.getSAeFormsCollectionNotice(),
        };
      default:
        throw new Error('Document type does not exist');
    }
  }

  public getPIdPCollectionNotice(): string {
    return `
      The Provider Identity Portal collects personal information for the purposes of verification and access
      to participating health systems. This is collected by the Ministry of Health under sections 26(c) and
      27(1)(b) of the Freedom of Information and Protection of Privacy Act. Should you have any questions
      about the collection of this personal information, contact
      <a href="mailto:${this.config.emails.providerIdentitySupport}">${this.config.emails.providerIdentitySupport}</a>.
    `;
  }

  public getSAeFormsCollectionNotice(): string {
    return `
      The personal information you provide to enrol for access to the Special Authority eForms application
      is collected by the British Columbia Ministry of Health under the authority of s. 26(a) and 26(c) of
      the Freedom of Information and Protection of Privacy Act (FOIPPA) and s. 22(1)(b) of the Pharmaceutical
      Services Act for the purpose of managing your access to, and use of, the Special Authority eForms
      application. If you have any questions about the collection or use of this information, contact
      <a href="mailto:${this.config.emails.specialAuthoritySupport}">${this.config.emails.specialAuthoritySupport}</a>.
    `;
  }

  private getDocumentMetaData(documentType: DocumentType): IDocumentMetaData {
    const metadata = this.documents.find(
      (document) => document.type === documentType
    );

    if (!metadata) {
      throw new Error('Document does not exist');
    }

    return metadata;
  }
}
