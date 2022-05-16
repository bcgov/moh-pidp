import { Address } from './address.model';
import { Facility } from './facility.model';
import { PartyCertification } from './party-certification.model';
import { User } from './user.model';

export enum AccessType {
  SAEforms = 1,
  HcimAccountTransfer,
  HcimEnrolment,
  DriverFitness,
}

export const AccessTypeMap: { [AccessType: number]: string } = {
  [AccessType.SAEforms]: 'Special Authority eForms',
  [AccessType.HcimAccountTransfer]: 'HCIMWeb Account Transfer',
  [AccessType.HcimEnrolment]: 'HCIMWeb Enrolment',
  [AccessType.DriverFitness]: 'Driver Medical Fitness',
};

export interface AccessRequest {
  id: number;
  partyId: number;
  requestedOn: string;
  accessType: AccessType;
}

export interface Party extends User {
  id?: number;
  // TODO should be off BcscUser not User but need Auth lib to share
  //      which contains a base BcscUser interface with userId
  // userId: string;
  // hpdid: string;
  preferredFirstName: string;
  preferredMiddleName: string;
  preferredLastName: string;
  dateOfBirth: string;
  email: string;
  phone: string;
  mailingAddress: Address;
  partyCertification: PartyCertification;
  jobTitle: string;
  facility: Facility;
  accessRequests: AccessRequest[];
}
