import { AccountExistsValidator } from './account-exists.directive';

describe('AccountExistsDirective', () => {
  it('should create an instance', () => {
    const directive = new AccountExistsValidator(null, null);
    expect(directive).toBeTruthy();
  });
});
