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
        },
        demographics: {
          statusCode: StatusCode.AVAILABLE,
        },
        collegeCertification: {
          hasCpn: false,
          licenceDeclared: false,
          statusCode: StatusCode.AVAILABLE,
          isComplete: false,
        },
        accountLinking: { statusCode: StatusCode.AVAILABLE },
        endorsements: { statusCode: StatusCode.AVAILABLE },
        userAccessAgreement: { statusCode: StatusCode.AVAILABLE },
        saEforms: {
          statusCode: StatusCode.AVAILABLE,
          incorrectLicenceType: false,
        },
        prescriptionRefillEforms: { statusCode: StatusCode.AVAILABLE },
        'prescription-refill-eforms': { statusCode: StatusCode.AVAILABLE },
        bcProvider: { statusCode: StatusCode.AVAILABLE },
        hcimAccountTransfer: { statusCode: StatusCode.AVAILABLE },
        driverFitness: { statusCode: StatusCode.AVAILABLE },
        msTeamsPrivacyOfficer: { statusCode: StatusCode.AVAILABLE },
        msTeamsClinicMember: { statusCode: StatusCode.AVAILABLE },
        providerReportingPortal: { statusCode: StatusCode.AVAILABLE },
        'provider-reporting-portal': { statusCode: StatusCode.AVAILABLE },
        complianceTraining: { statusCode: StatusCode.AVAILABLE },
        provincialAttachmentSystem: { statusCode: StatusCode.AVAILABLE },
        immsBCEforms: { statusCode: StatusCode.AVAILABLE },
      },
    };
  }
}
