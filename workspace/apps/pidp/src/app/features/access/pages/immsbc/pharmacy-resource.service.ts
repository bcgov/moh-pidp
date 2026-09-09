import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

import { Observable } from 'rxjs';

import {
  IStaff,
  Pharmacy,
  PharmacyProfile,
  PharmacyRole,
} from './pharmacy-staff.model';

@Injectable({
  providedIn: 'root',
})
export class PharmacyResource {
  private readonly http = inject(HttpClient);

  private readonly apiEndpoint = '/api/pharmacies';

  public getPharmacyAdminProfile(): Observable<PharmacyProfile> {
    return this.http.get<PharmacyProfile>('/api/pharmacies/profile');
  }

  public getPharmacyDetails(pharmacyId: number): Observable<Pharmacy | null> {
    return this.http.get<Pharmacy | null>(`${this.apiEndpoint}/${pharmacyId}`);
  }

  public createPharmacy(payload: unknown): Observable<number> {
    return this.http.post<number>(this.apiEndpoint, payload);
  }

  public updatePharmacy(
    pharmacyId: number,
    payload: Partial<Pharmacy>,
  ): Observable<void> {
    payload.id = pharmacyId;
    return this.http.put<void>(`${this.apiEndpoint}/${pharmacyId}`, payload);
  }

  public searchPharmacies(query: string): Observable<Pharmacy[]> {
    return this.http.get<Pharmacy[]>(`${this.apiEndpoint}/search`, {
      params: { query },
    });
  }

  public searchManager(licenceNumber: string): Observable<{ partyId: number; fullName: string }> {
    return this.http.get<{ partyId: number; fullName: string }>(`${this.apiEndpoint}/manager-search`, {
      params: { licenceNumber },
    });
  }

  public claimPharmacy(pharmacyId: number): Observable<void> {
    return this.http.post<void>(`${this.apiEndpoint}/${pharmacyId}/claim`, {});
  }

  public getStaff(pharmacyId: number): Observable<IStaff[]> {
    return this.http.get<IStaff[]>(`${this.apiEndpoint}/${pharmacyId}/staff`);
  }

  public getStaffDetails(
    pharmacyId: number,
    partyId: number,
  ): Observable<IStaff> {
    return this.http.get<IStaff>(`${this.apiEndpoint}/${pharmacyId}/staff/${partyId}`);
  }

  public generateEnrolmentToken(
    pharmacyId: number,
    role: PharmacyRole
  ): Observable<string> {
    return this.http.get(`${this.apiEndpoint}/${pharmacyId}/enrolment-token`, {
      params: { role: role.toString() },
      responseType: 'text',
    });
  }

  public inviteStaff(
    pharmacyId: number,
    role: PharmacyRole,
    emails: string[]
  ): Observable<void> {
    return this.http.post<void>(`${this.apiEndpoint}/${pharmacyId}/invite`, {
      roleToAssign: role,
      emails: emails
    });
  }

  public enrolStaff(token: string, payload: { privacyTrainingAcknowledged: boolean }): Observable<void> {
    return this.http.post<void>(`${this.apiEndpoint}/enrolments/${token}`, payload);
  }

  public deleteStaff(
    pharmacyId: number,
    partyId: number
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.apiEndpoint}/${pharmacyId}/staff/${partyId}`
    );
  }

  public updateStaff(
    pharmacyId: number,
    partyId: number,
    payload: {
      role?: PharmacyRole;
      effectiveStartDate?: string | null;
      effectiveEndDate?: string | null;
    }
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiEndpoint}/${pharmacyId}/staff/${partyId}`,
      payload
    );
  }
}