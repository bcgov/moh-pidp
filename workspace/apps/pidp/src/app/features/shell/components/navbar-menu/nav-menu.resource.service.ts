import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiHttpClient } from "@app/core/resources/api-http-client.service";
import { catchError, Observable, throwError } from "rxjs";
import { Credential } from "./nav-menu.model";

@Injectable({
  providedIn: 'root',
})
export class NavMenuResource {
  public constructor(
    protected apiResource: ApiHttpClient
  ) {}

  public getCredentials(partyId: number): Observable<Credential[]> {
    return this.apiResource
      .get<Credential[]>(this.getResourcePath(partyId))
      .pipe(
        catchError((error: HttpErrorResponse) => {
          return throwError(() => error);
        }),
      );
  }

  protected getResourcePath(partyId: number): string {
    return `parties/${partyId}/credentials`;
  }
}
