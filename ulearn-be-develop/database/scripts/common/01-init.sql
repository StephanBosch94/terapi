create function get_UTC_time()
returns timestamp as
$$
  select now() at time zone 'utc';
$$
language sql;

SET TIME ZONE 'Europe/Prague';

insert into app_user (
    login,                              email,                      name,                   first_name,     last_name,      created,        user_state_id,  iana_timezone,      user_role_id,   password,                                       country_id, currency_id,    hour_rate,  subject_id,         teaching_experience_id, excluded,   ico,            place_of_business,                                      company_name,       billing_email,          registration_method_id) values
    ('technical.admin@yaspeak.com',     'support@yaspeak.com',      'Yaspeak Platform',     'Yaspeak.com',  '',             get_UTC_time(), 5,              'Europe/Prague',    1,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 45,         1,              NULL,       NULL,               NULL,                   true,       '127130140',    'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               NULL,                   1),
    ('technical.tutor@yaspeak.com',     'support@yaspeak.com',      'Yaspeak Tutor',        'Yaspeak',      'Tutor',        get_UTC_time(), 5,              'Europe/Prague',    2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 45,         1,              500,        1,                  2,                      true,       '126130140',    'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               NULL,                   1),
    ('technical.student@yaspeak.com',   'support@yaspeak.com',      'Yaspeak Student',      'Yaspeak',      'Student',      get_UTC_time(), 5,              'Europe/Prague',    3,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 45,         1,              NULL,       NULL,               NULL,                   true,       null,           null,                                                   NULL,               NULL,                   1),
    ('technical.company@yaspeak.com',   'support@yaspeak.com',      'Yaspeak Company',      'Yaspeak',      'Company',      get_UTC_time(), 5,              'Europe/Prague',    4,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 45,         1,              NULL,       NULL,               NULL,                   true,       '126130140',    'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    'Yaspeak',          'support@yaspeak.com',  1);

insert into app_user_wallet (user_id, currency_id, amount, scheduled_amount, updated, bank_account_number, bank_code)
select u.user_id, 1, 0, 0, current_timestamp, '720140217', '0100'
from app_user u
         left join app_user_wallet w on u.user_id = w.user_id
where w.user_wallet_id is null;

insert into r_user_language(user_id, language_id, language_level_id, is_default) 
select user_id, language_id, language_level_id, CAST(0 AS boolean)
    from app_user u 
    inner join c_language l on (l.enum_key in ('english'))
    inner join c_language_level ll on (ll.enum_key = 'native')
    where login = 'technical.tutor@yaspeak.com';

insert into c_translation_namespace (name, created, updated, creator_id, updater_id) 
select 'default', get_UTC_time(), get_UTC_time(), user_id, user_id
    from app_user 
    where login = 'technical.admin@yaspeak.com';

insert into c_translation_language (language_code, language) values ('en', 'English');
insert into c_translation_language (language_code, language) values ('ru', 'Русский');
insert into c_translation_language (language_code, language) values ('uk', 'Українська');
insert into c_translation_language (language_code, language) values ('sr', 'Srpski');

insert into c_master_hash (table_name, hash, enum_key, name_key, description_key, updated)
select
    mh.table_name,
    mh.hash,
    concat(mh.enum_key, '_LANG_', tl.language_code),
    concat('master_hash.', mh.enum_key, '_LANG_', tl.language_code, '.name'),
    concat('master_hash.', mh.enum_key, '_LANG_', tl.language_code, '.description'),
    get_UTC_time()
from c_translation_language tl
         inner join c_master_hash mh on (mh.enum_key = 'c_translation');

insert into app_coupon (coupon_type_id, discount_percentage, created, valid_from, valid_to, code) values
(1, 10, current_timestamp, '2023-09-01T00:00:00', '2023-12-01T00:00:00', 'PODZIM2023'),
(1, 10, current_timestamp, '2023-12-01T00:00:00', '2024-03-01T00:00:00', 'ZIMA2024'),
(1, 10, current_timestamp, '2024-03-01T00:00:00', '2024-06-01T00:00:00', 'JARO2024'),
(1, 10, current_timestamp, '2024-06-01T00:00:00', '2024-09-01T00:00:00', 'LETO2024'),
(1, 10, current_timestamp, '2024-09-01T00:00:00', '2024-12-01T00:00:00', 'PODZIM2024'),
(1, 10, current_timestamp, '2024-12-01T00:00:00', '2025-03-01T00:00:00', 'ZIMA2025');