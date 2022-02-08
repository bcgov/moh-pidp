export abstract class AbstractResource {
  protected readonly resourceBaseUri: string;

  protected constructor(resourceBaseUri: string) {
    this.resourceBaseUri = resourceBaseUri;
  }
}
