import { SelfDeclarationType } from './self-declaration.enum';

export const selfDeclarationQuestions = {
  // Limits or conditions
  [SelfDeclarationType.LIMITS_OR_CONDITIONS]: `Are you, or have you ever been, subject to any limits,
    conditions or prohibitions imposed as a result of disciplinary actions taken by a governing body of a health
    profession in any jurisdiction, that involved improper access to, collection, use, or disclosure of personal
    information?`,
  // Conviction
  [SelfDeclarationType.HAS_CONVICTION]: `Are you, or have you ever been, the subject of an order or a
    conviction under legislation in any jurisdiction for a matter that involved improper access to, collection,
    use, or disclosure of personal information?`,
  // Disciplined or fired
  [SelfDeclarationType.DISCIPLINED_OR_FIRED]: `Have you ever been disciplined or fired by an employer, or had
    a contract for your services terminated, for a matter that involved improper access to, collection, use, or
    disclosure of personal information?`,
  // Access suspended or cancelled
  [SelfDeclarationType.SUSPENDED_OR_CANCELLED]: `Have you ever had your access to an electronic health record
    system, electronic medical record system, pharmacy or laboratory record system, or any similar health information
    system, in any jurisdiction, suspended or cancelled?`,
};
