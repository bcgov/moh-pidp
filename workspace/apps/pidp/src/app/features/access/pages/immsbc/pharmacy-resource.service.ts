import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

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
  private apiEndpoint = '/api/pharmacies';

  public constructor(private http: HttpClient) {}

  public getPharmacyAdminProfile(): Observable<PharmacyProfile> {
    return this.http.get<PharmacyProfile>('/api/pharmacies/profile');
  }

  public getPharmacyDetails(pharmacyId: number): Observable<Pharmacy | null> {
    return this.http.get<Pharmacy | null>(`${this.apiEndpoint}/${pharmacyId}`);
  }

  public updatePharmacy(
    pharmacyId: number,
    payload: Partial<Pharmacy>,
  ): Observable<void> {
    return this.http.put<void>(`${this.apiEndpoint}/${pharmacyId}`, payload);
  }

  public getStaff(pharmacyId: number): Observable<IStaff[]> {
    return this.http.get<IStaff[]>(`${this.apiEndpoint}/${pharmacyId}/staff`);
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