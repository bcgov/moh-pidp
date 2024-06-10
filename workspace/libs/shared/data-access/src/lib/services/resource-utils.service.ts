import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ResourceUtilsService {
  /**
   * @description
   * Make HttpParams from an object literal.
   */
  public makeHttpParams(queryParams: {
    [key: string]: unknown;
  }): HttpParams | null {
    return queryParams
      ? Object.keys(queryParams).reduce(
          (httpParams: HttpParams, key: string) =>
            !queryParams[key] !== null && !queryParams[key] !== undefined
              ? this.createHttpParam(httpParams, key, queryParams[key])
              : httpParams,
          new HttpParams(),
        )
      : null;
  }

  /**
   * @description
   * Recursively append HTTP params.
   */
  private createHttpParam(
    httpParams: HttpParams,
    key: string,
    value: unknown,
  ): HttpParams {
    return Array.isArray(value)
      ? value.reduce((h, b) => this.createHttpParam(h, key, b), httpParams)
      : httpParams.append(key, `${value}`);
  }
}
