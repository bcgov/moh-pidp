import { randFullName, randNumber } from '@ngneat/falso';

import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';

export class MockProfileStatus {
  public static get(): ProfileStatus {
    return {
      alerts: [],
      status: {
        dashboardInfo: {
          displayFullName: randFullName(),
          collegeCode: randNumber(),
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'Dashboard information is complete',
        },
        demographics: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'demographics information is complete',
        },
        collegeCertification: {
          hasCpn: false,
          licenceDeclared: false,
          statusCode: StatusCode.AVAILABLE,
          isComplete: false,
          completedMessage: 'college certification information is complete',
        },
        accountLinking: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'account linking information is complete',
        },
        endorsements: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'endorsements information is complete',
        },
        userAccessAgreement: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'userAccessAgreement information is complete',
        },
        saEforms: {
          statusCode: StatusCode.AVAILABLE,
          incorrectLicenceType: false,
          completedMessage: 'saEforms information is complete',
        },
        prescriptionRefillEforms: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'prescriptionRefillEforms information is complete',
        },
        'prescription-refill-eforms': {
          statusCode: StatusCode.AVAILABLE,
          completedMessage:
            'prescription-refill-eforms information is complete',
        },
        bcProvider: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'bcProvider information is complete',
        },
        hcimAccountTransfer: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'hcimAccountTransfer information is complete',
        },
        driverFitness: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'driverFitness information is complete',
        },
        msTeamsPrivacyOfficer: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'msTeamsPrivacyOfficer information is complete',
        },
        msTeamsClinicMember: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'msTeamsClinicMember information is complete',
        },
        providerReportingPortal: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'providerReportingPortal information is complete',
        },
        'provider-reporting-portal': {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'provider-reporting-portal information is complete',
        },
        complianceTraining: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'complianceTraining information is complete',
        },
        provincialAttachmentSystem: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage:
            'provincialAttachmentSystem information is complete',
        },
        immsBCEforms: {
          statusCode: StatusCode.AVAILABLE,
          completedMessage: 'immsBCEforms information is complete',
        },
      },
    };
  }
}
