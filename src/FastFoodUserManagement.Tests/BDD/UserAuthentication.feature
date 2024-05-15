Feature: UserAuthentication
    As a user
    I want to be able to authenticate myself
    So that I can access secure features

Scenario: Successful user authentication
    Given a user with CPF "352.932.590-24" exists in the system
    When the user attempts to authenticate
    Then the system should return a token
    And the token should be valid