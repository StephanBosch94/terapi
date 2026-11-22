create or REPLACE function get_PRG_time()
    returns timestamp as
$$
select now() at time zone 'Europe/Prague';
$$
    language sql;

create or REPLACE function get_truncated_week_PRG_time()
    returns timestamp as
$$
select date_trunc('week', get_PRG_time());
$$
    language sql;

create or REPLACE function get_truncated_day_PRG_time()
    returns timestamp as
$$
select date_trunc('day', get_PRG_time());
$$
    language sql;

CREATE OR REPLACE FUNCTION get_reservation_json(reservation_id_in int, notification_type_in varchar)
    RETURNS varchar AS $$
DECLARE
    result varchar;
BEGIN
    SELECT CAST(CONCAT(
            '{',
            '"type": "',notification_type_in,'",',
            '"reservationId": ',r1.reservation_id,', ',
            '"startTime": "',r1.start_time,'", ',
            '"endTime": "',r1.end_time,'", ',
            '"expectedEndTime": "',r1.expected_end_time,'", ',
            '"sourceStartTime": ', CASE WHEN r1_source.start_time IS NOT NULL THEN CONCAT('"', r1_source.start_time, '",') ELSE 'null,' END,
            '"sourceEndTime": ', CASE WHEN r1_source.end_time IS NOT NULL THEN CONCAT('"', r1_source.end_time, '",') ELSE 'null,' END,
            '"sourceExpectedEndTime": ', CASE WHEN r1_source.expected_end_time IS NOT NULL THEN CONCAT('"', r1_source.expected_end_time, '",') ELSE 'null,' END,
            '"roomKey": "',r1_room.room_key,'"',
            '}') AS varchar)
    INTO result
    FROM app_reservation r1
             INNER JOIN app_room r1_room ON r1.room_id = r1_room.room_id
             LEFT JOIN app_reservation r1_source on r1.source_reservation_id = r1_source.reservation_id
    WHERE r1.reservation_id = reservation_id_in
    OFFSET 0 ROWS LIMIT 1;

    RETURN result;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_reservation_issue_json(reservation_issue_in int, notification_type_in varchar, recipient_id_in int)
    RETURNS varchar AS $$
DECLARE
    result varchar;
BEGIN
    SELECT CAST(CONCAT(
            '{',
            '"type": "',notification_type_in,'",',
            '"reservationId": ',r.reservation_id,', ',
            '"reservationStartTime": "',r.start_time,'", ',
            '"reservationEndTime": "',r.end_time,'", ',
            '"reservationExpectedEndTime": "',r.expected_end_time,'", ',
            '"reservationIssueId": ',i.reservation_issue_id,', ',
            '"created": "',i.created,'", ',
            '"issueTypeNameKey": "',t.name_key,'", ',
            '"issueDescription": "',i.description,'", ',
            '"refundAmount": ', CASE WHEN recipient_id_in = r.host_id THEN CAST(COALESCE(ttr.amount, 0.0) as varchar) ELSE CAST(COALESCE(str.amount, 0.0) as varchar) END,', ',
            '"refundAmountCurrencyNameKey": "', CASE WHEN recipient_id_in = r.host_id THEN COALESCE(ttr_currency.name_key,'') ELSE COALESCE(str_currency.name_key, '') END,'", ',
            '"resolved": ', CASE WHEN i.resolved IS NOT NULL THEN CONCAT('"', i.resolved, '",') ELSE 'null,' END,
            '"decisionText": ', CASE WHEN i.decision_text IS NOT NULL THEN CONCAT('"', i.decision_text, '",') ELSE 'null,' END,
            '}') AS varchar)
    INTO result
    FROM app_reservation_issue i
             INNER JOIN app_reservation r ON i.reservation_id = r.reservation_id
             INNER JOIN c_reservation_issue_type t ON i.reservation_issue_type_id = t.reservation_issue_type_id
             LEFT JOIN app_transaction ttr ON ttr.reservation_issue_id = i.reservation_issue_id AND ttr.transaction_type_id = 9
             LEFT JOIN app_transaction str ON str.reservation_issue_id = i.reservation_issue_id AND str.transaction_type_id = 5
             LEFT JOIN c_currency ttr_currency ON ttr.currency_id = ttr_currency.currency_id
             LEFT JOIN c_currency str_currency ON str.currency_id = str_currency.currency_id
    WHERE i.reservation_issue_id = reservation_issue_in
    OFFSET 0 ROWS LIMIT 1;

    RETURN result;
END;
$$ LANGUAGE plpgsql;

insert into app_user
(login,                                 email,                                  name,                   first_name, last_name,      created,            user_state_id,  iana_timezone,    user_role_id,             password,                                       country_id, currency_id,    hour_rate,  subject_id,     teaching_experience_id,         excluded,        translation_language_id,   weekly_goal_id,     phone,              ico,            dic,                   is_vat_payer,        place_of_business,                                      company_name,       billing_email,          hubspot_contact_id,     certifications_verified,    registration_method_id           ) values
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('emma.smith@yaspeak.com',              'yaspeaktesting@gmail.com',             'Emma Smith',           'Emma',     'Smith',        current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              500,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    null,               null,                   1151,                   true,                       1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('britney.johansberg@yaspeak.com',      'yaspeaktesting@gmail.com',             'Britney Johansberg',   'Britney',  'Johansberg',   current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              700,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    null,               null,                   1151,                   true,                       1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('alice.anderson@yaspeak.com',          'yaspeaktesting@gmail.com',             'Alice Anderson',       'Alice',    'Anderson',     current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              300,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    null,               null,                   1151,                   true,                       1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('emily.jick@yaspeak.com',              'yaspeaktesting@gmail.com',             'Emily Jick',           'Emily',    'Jick',         current_timestamp,  5,              'Europe/Prague',            3,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              NULL,       1,              1,                              false,          1,                          2,                  '+420720120750',     null,          null,                  false,               null,                                                   null,               null,                   1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('adam.stone@yaspeak.com',              'yaspeaktesting@gmail.com',             'Adam Stone',           'Adam',     'Stone',        current_timestamp,  5,              'Europe/Prague',            3,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              NULL,       1,              1,                              false,          1,                          2,                  '+420720120750',     null,          null,                  false,               null,                                                   null,               null,                   1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('rebecca.moore@yaspeak.com',           'yaspeaktesting@gmail.com',             'Rebecca Moore',        'Rebecca',  'Moore',        current_timestamp,  5,              'Europe/Prague',            3,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              NULL,       1,              1,                              false,          1,                          3,                  '+420720120750',     null,          null,                  false,               null,                                                   null,               null,                   1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('alisa.orlova@yaspeak.com',            'yaspeaktesting@gmail.com',             'Alisa Orlova',         'Alisa',    'Orlova',       current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              400,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('julia.berko@yaspeak.com',             'yaspeaktesting@gmail.com',             'Julia Berko',          'Julia',    'Berko',        current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              800,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('oksana.sonnova@yaspeak.com',          'yaspeaktesting@gmail.com',             'Oksana Sonnova',       'Oksana',   'Sonnova',      current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              200,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('jenny.santana@yaspeak.com',           'yaspeaktesting@gmail.com',             'Jenny Santana',		'Jenny',    'Santana',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              900,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('jerome.coffey@yaspeak.com',		 	'yaspeaktesting@gmail.com',             'Jerome Coffey',		'Jerome',   'Coffey',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              700,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('ayden.townsend@yaspeak.com',          'yaspeaktesting@gmail.com',             'Ayden Townsend',		'Ayden',    'Townsend',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              550,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('cassie.pope@yaspeak.com',             'yaspeaktesting@gmail.com',             'Cassie Pope',			'Cassie',   'Pope',			current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              600,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('jazmin.rodgers@yaspeak.com',          'yaspeaktesting@gmail.com',             'Jazmin Rodgers',		'Jazmin',   'Rodgers',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              730,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('lyra.larsen@yaspeak.com',             'yaspeaktesting@gmail.com',             'Lyra Larsen',			'Lyra',     'Larsen',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              820,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('vera.mcgowan@yaspeak.com',		 	'yaspeaktesting@gmail.com',             'Vera Mcgowan',			'Vera',     'Mcgowan',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              950,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('chad.rivas@yaspeak.com',              'yaspeaktesting@gmail.com',             'Chad Rivas',			'Chad',     'Rivas',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              600,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('lance.garcia@yaspeak.com',            'yaspeaktesting@gmail.com',             'Lance Garcia',			'Lance',    'Garcia',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              650,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('sumaiya.beltran@yaspeak.com',         'yaspeaktesting@gmail.com',             'Sumaiya Beltran',		'Sumaiya',  'Beltran',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              530,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('delilah.shannon@yaspeak.com',         'yaspeaktesting@gmail.com',             'Delilah Shannon',		'Delilah',  'Shannon',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              540,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('alyssia.owen@yaspeak.com',			'yaspeaktesting@gmail.com',             'Alyssia Owen',			'Alyssia',  'Owen',			current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              230,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('tyrell.meadows@yaspeak.com',          'yaspeaktesting@gmail.com',             'Tyrell Meadows',		'Tyrell',   'Meadows',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              700,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('craig.bender@yaspeak.com',			'yaspeaktesting@gmail.com',             'Craig Bender',			'Craig',    'Bender',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              220,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('mae.ali@yaspeak.com',                 'yaspeaktesting@gmail.com',             'Mae Ali',				'Mae',      'Ali',			current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              350,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('malakai.cherry@yaspeak.com',          'yaspeaktesting@gmail.com',             'Malakai Cherry',		'Malakai',  'Cherry',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              370,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('krishan.foley@yaspeak.com',			'yaspeaktesting@gmail.com',             'Krishan Foley',		'Krishan',  'Foley',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              460,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('denzel.mcclain@yaspeak.com',          'yaspeaktesting@gmail.com',             'Denzel Mcclain',		'Denzel',   'Mcclain',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              920,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('justin.miller@yaspeak.com',			'yaspeaktesting@gmail.com',             'Justin Miller',		'Justin',   'Miller',		current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              530,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('libbie.king@yaspeak.com',             'yaspeaktesting@gmail.com',             'Libbie King',			'Libbie',   'King',			current_timestamp,  5,              'Europe/Prague',            2,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              570,        1,              1,                              false,          1,                          null,               '+420720120750',     '127130140',   null,                  false,               'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    NULL,               'support@yaspeak.com',  1151,                   false,                      1         ),
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ('ud4d@yaspeak.com',                    'yaspeaktesting@gmail.com',             NULL,		            NULL,       NULL,           current_timestamp,  5,              'Europe/Prague',            4,              'Cl3DHI9DxGm/KWGSmiDNAIhcZfFP9SrAZQjWd4Qos/k=', 54,         1,              NULL,       NULL,           NULL,                           false,          1,                          null,               '+420720120750',     '127130140',   'CZ8163295218',        true,                'Příčná 1892/4, Nové Město (Praha 1), 110 00 Praha',    'UD4D',             'support@yaspeak.com',  1151,                   false,                      1         )
;

insert into app_user_wallet (user_id, currency_id, amount, scheduled_amount, total_revenue, updated, bank_account_number, bank_code)
select u.user_id, 1, 0, 0, 0, current_timestamp, '720140217', '0100'
from app_user u
         left join app_user_wallet w on u.user_id = w.user_id
where w.user_wallet_id is null;

-- SPECIALIZATIONS
with specializations (login, specialization) as (
    SELECT 'alice.anderson@yaspeak.com',                'business_english'          UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'english_intensive_course'           UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'conversational_english'   UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'english_from_zero'          UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'toeic_preparation'      UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'conversational_english'   UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'support_education'     UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'business_english'          UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'english_intensive_course'           UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'conversational_english'   UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'toeic_preparation'      UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'english_from_zero'          UNION ALL
    SELECT 'emily.jick@yaspeak.com',                    'english_from_zero'          UNION ALL
    SELECT 'emily.jick@yaspeak.com',                    'toeic_preparation'      UNION ALL
    SELECT 'adam.stone@yaspeak.com',                    'english_intensive_course'           UNION ALL
    SELECT 'adam.stone@yaspeak.com',                    'business_english'          UNION ALL
    SELECT 'rebecca.moore@yaspeak.com',                 'support_education'     UNION ALL
    SELECT 'alisa.orlova@yaspeak.com',                  'support_education'		UNION ALL
    SELECT 'julia.berko@yaspeak.com', 		            'business_english'			UNION ALL
    SELECT 'oksana.sonnova@yaspeak.com', 		        'english_intensive_course'			UNION ALL
    SELECT 'jenny.santana@yaspeak.com', 		        'business_english'			UNION ALL
    SELECT 'jerome.coffey@yaspeak.com', 		        'support_education'		UNION ALL
    SELECT 'ayden.townsend@yaspeak.com', 		        'business_english'			UNION ALL
    SELECT 'cassie.pope@yaspeak.com', 		            'support_education'		UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'support_education'		UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'english_intensive_course'			UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'toeic_preparation'		UNION ALL
    SELECT 'lyra.larsen@yaspeak.com', 		            'english_intensive_course'			UNION ALL
    SELECT 'vera.mcgowan@yaspeak.com', 		            'support_education'		UNION ALL
    SELECT 'chad.rivas@yaspeak.com', 			        'business_english'			UNION ALL
    SELECT 'lance.garcia@yaspeak.com', 		            'support_education'		UNION ALL
    SELECT 'sumaiya.beltran@yaspeak.com', 	            'language_for_kids'		UNION ALL
    SELECT 'delilah.shannon@yaspeak.com',		        'support_education'		UNION ALL
    SELECT 'alyssia.owen@yaspeak.com', 		            'english_intensive_course'			UNION ALL
    SELECT 'tyrell.meadows@yaspeak.com', 		        'business_english'			UNION ALL
    SELECT 'craig.bender@yaspeak.com', 		            'toeic_preparation'		UNION ALL
    SELECT 'mae.ali@yaspeak.com', 			            'language_for_kids'		UNION ALL
    SELECT 'malakai.cherry@yaspeak.com', 		        'support_education'		UNION ALL
    SELECT 'krishan.foley@yaspeak.com', 		        'business_english'			UNION ALL
    SELECT 'denzel.mcclain@yaspeak.com', 		        'toeic_preparation'		UNION ALL
    SELECT 'justin.miller@yaspeak.com', 		        'business_english'			UNION ALL
    SELECT 'libbie.king@yaspeak.com', 		            'language_for_kids'		UNION ALL
    SELECT 'libbie.king@yaspeak.com', 		            'english_from_zero'          UNION ALL
    SELECT 'emily.jick@yaspeak.com', 		            'support_education'     UNION ALL
    SELECT 'adam.stone@yaspeak.com', 		            'support_education'     UNION ALL
    SELECT 'rebecca.moore@yaspeak.com',                 'conversational_english'
)
INSERT INTO r_user_specialization (user_id, specialization_id)
SELECT u.user_id, s.specialization_id
FROM specializations tmp_s
         INNER JOIN c_specialization s on (tmp_s.specialization = s.enum_key)
         INNER JOIN app_user u on (tmp_s.login = u.login);

-- INTERESTS
with interests (login, interest) as (
    SELECT 'alice.anderson@yaspeak.com',                'cooking'			UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'travel'			UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'cars'				UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'workout'			UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'history'			UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'music'			    UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'writing'			UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'personal_growth'	UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'cars'				UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'photography'		UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'cooking'			UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'travel'			UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'cars'				UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'personal_growth'	UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'writing'			UNION ALL
    SELECT 'emily.jick@yaspeak.com',                    'writing'			UNION ALL
    SELECT 'emily.jick@yaspeak.com',                    'football'			UNION ALL
    SELECT 'emily.jick@yaspeak.com',                    'personal_growth'	UNION ALL
    SELECT 'adam.stone@yaspeak.com',                    'travel'			UNION ALL
    SELECT 'adam.stone@yaspeak.com',                    'cooking'			UNION ALL
    SELECT 'rebecca.moore@yaspeak.com',                 'photography'		UNION ALL
    SELECT 'alisa.orlova@yaspeak.com', 		            'photography'		UNION ALL
    SELECT 'julia.berko@yaspeak.com', 		            'cooking'			UNION ALL
    SELECT 'oksana.sonnova@yaspeak.com', 		        'travel'			UNION ALL
    SELECT 'jenny.santana@yaspeak.com', 		        'cooking'			UNION ALL
    SELECT 'jerome.coffey@yaspeak.com', 		        'photography'		UNION ALL
    SELECT 'ayden.townsend@yaspeak.com', 		        'cooking'			UNION ALL
    SELECT 'cassie.pope@yaspeak.com', 		            'photography'		UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'photography'		UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'travel'			UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'personal_growth'	UNION ALL
    SELECT 'lyra.larsen@yaspeak.com', 		            'travel'			UNION ALL
    SELECT 'vera.mcgowan@yaspeak.com', 		            'photography'		UNION ALL
    SELECT 'chad.rivas@yaspeak.com', 			        'cooking'			UNION ALL
    SELECT 'lance.garcia@yaspeak.com', 		            'photography'		UNION ALL
    SELECT 'sumaiya.beltran@yaspeak.com', 	            'meditation'		UNION ALL
    SELECT 'delilah.shannon@yaspeak.com',		        'photography'		UNION ALL
    SELECT 'alyssia.owen@yaspeak.com', 		            'travel'			UNION ALL
    SELECT 'tyrell.meadows@yaspeak.com', 		        'cooking'			UNION ALL
    SELECT 'craig.bender@yaspeak.com', 		            'personal_growth'	UNION ALL
    SELECT 'mae.ali@yaspeak.com', 			            'meditation'		UNION ALL
    SELECT 'malakai.cherry@yaspeak.com', 		        'photography'		UNION ALL
    SELECT 'krishan.foley@yaspeak.com', 		        'cooking'			UNION ALL
    SELECT 'denzel.mcclain@yaspeak.com', 		        'personal_growth'	UNION ALL
    SELECT 'justin.miller@yaspeak.com', 		        'cooking'			UNION ALL
    SELECT 'libbie.king@yaspeak.com', 		            'meditation'		UNION ALL
    SELECT 'libbie.king@yaspeak.com', 		            'writing'			UNION ALL
    SELECT 'emily.jick@yaspeak.com', 		            'photography'		UNION ALL
    SELECT 'adam.stone@yaspeak.com', 		            'photography'		UNION ALL
    SELECT 'rebecca.moore@yaspeak.com',                 'cars'
)
INSERT INTO r_user_interest (user_id, interest_id)
SELECT u.user_id, s.interest_id
FROM interests tmp_s
         INNER JOIN c_interest s on (tmp_s.interest = s.enum_key)
         INNER JOIN app_user u on (tmp_s.login = u.login);

-- SUBJECT LEVELS
with subject_levels (login, subject_level) as (
    SELECT 'alice.anderson@yaspeak.com',                'beginner'          UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'intermediate'      UNION ALL
    SELECT 'alice.anderson@yaspeak.com',                'advanced'          UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'beginner'          UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'intermediate'      UNION ALL
    SELECT 'emma.smith@yaspeak.com',                    'advanced'          UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'beginner'          UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'intermediate'      UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',            'advanced'          UNION ALL
    SELECT 'emily.jick@yaspeak.com',                    'beginner'          UNION ALL
    SELECT 'emily.jick@yaspeak.com',                    'intermediate'      UNION ALL
    SELECT 'adam.stone@yaspeak.com',                    'beginner'          UNION ALL
    SELECT 'adam.stone@yaspeak.com',                    'intermediate'      UNION ALL
    SELECT 'rebecca.moore@yaspeak.com',                 'beginner'          UNION ALL
    SELECT 'alisa.orlova@yaspeak.com', 		            'beginner'		    UNION ALL
    SELECT 'julia.berko@yaspeak.com', 		            'beginner'			UNION ALL
    SELECT 'oksana.sonnova@yaspeak.com', 		        'beginner'			UNION ALL
    SELECT 'jenny.santana@yaspeak.com', 		        'beginner'			UNION ALL
    SELECT 'jerome.coffey@yaspeak.com', 		        'beginner'		    UNION ALL
    SELECT 'ayden.townsend@yaspeak.com', 		        'beginner'			UNION ALL
    SELECT 'cassie.pope@yaspeak.com', 		            'beginner'		    UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'beginner'		    UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'intermediate'		UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com', 		        'advanced'		    UNION ALL
    SELECT 'lyra.larsen@yaspeak.com', 		            'beginner'			UNION ALL
    SELECT 'vera.mcgowan@yaspeak.com', 		            'beginner'		    UNION ALL
    SELECT 'chad.rivas@yaspeak.com', 			        'beginner'			UNION ALL
    SELECT 'lance.garcia@yaspeak.com', 		            'beginner'		    UNION ALL
    SELECT 'sumaiya.beltran@yaspeak.com', 	            'beginner'		    UNION ALL
    SELECT 'delilah.shannon@yaspeak.com',		        'beginner'		    UNION ALL
    SELECT 'alyssia.owen@yaspeak.com', 		            'beginner'			UNION ALL
    SELECT 'tyrell.meadows@yaspeak.com', 		        'beginner'			UNION ALL
    SELECT 'craig.bender@yaspeak.com', 		            'beginner'		    UNION ALL
    SELECT 'mae.ali@yaspeak.com', 			            'beginner'		    UNION ALL
    SELECT 'malakai.cherry@yaspeak.com', 		        'beginner'		    UNION ALL
    SELECT 'krishan.foley@yaspeak.com', 		        'beginner'			UNION ALL
    SELECT 'denzel.mcclain@yaspeak.com', 		        'beginner'		    UNION ALL
    SELECT 'justin.miller@yaspeak.com', 		        'beginner'			UNION ALL
    SELECT 'libbie.king@yaspeak.com', 		            'beginner'		    UNION ALL
    SELECT 'libbie.king@yaspeak.com', 		            'advanced'
)
INSERT INTO r_user_subject_level (user_id, subject_level_id)
SELECT u.user_id, s.subject_level_id
FROM subject_levels tmp_s
         INNER JOIN c_subject_level s on (tmp_s.subject_level = s.enum_key)
         INNER JOIN app_user u on (tmp_s.login = u.login);

-- LANGUAGES
with user_language (login, language, level) as (
    SELECT  'alice.anderson@yaspeak.com',           'english',      'native'        UNION ALL
    SELECT  'alice.anderson@yaspeak.com',           'swedish',      'native'        UNION ALL
    SELECT  'alice.anderson@yaspeak.com',           'italian',      'b2'            UNION ALL
    SELECT  'emma.smith@yaspeak.com',               'english',      'native'        UNION ALL
    SELECT  'emma.smith@yaspeak.com',               'spanish',      'c1'            UNION ALL
    SELECT  'britney.johansberg@yaspeak.com',       'english',      'native'        UNION ALL
    SELECT  'britney.johansberg@yaspeak.com',       'german',       'native'        UNION ALL
    SELECT  'britney.johansberg@yaspeak.com',       'greek',        'b1'            UNION ALL
    SELECT	'alisa.orlova@yaspeak.com',		        'russian',      'native'		UNION ALL
    SELECT	'alisa.orlova@yaspeak.com',		        'english',      'c2'			UNION ALL
    SELECT	'julia.berko@yaspeak.com',		        'english',      'c2'			UNION ALL
    SELECT	'julia.berko@yaspeak.com',		        'ukranian',     'native'		UNION ALL
    SELECT	'julia.berko@yaspeak.com',		        'russian',      'native'		UNION ALL
    SELECT	'oksana.sonnova@yaspeak.com',		    'english',      'c2'			UNION ALL
    SELECT	'jenny.santana@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'jerome.coffey@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'ayden.townsend@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'cassie.pope@yaspeak.com',		        'english',      'native'		UNION ALL
    SELECT	'jazmin.rodgers@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'lyra.larsen@yaspeak.com',		        'english',      'native'		UNION ALL
    SELECT	'vera.mcgowan@yaspeak.com',		        'english',      'native'		UNION ALL
    SELECT	'chad.rivas@yaspeak.com',			    'english',      'native'		UNION ALL
    SELECT	'chad.rivas@yaspeak.com',			    'danish',       'native'		UNION ALL
    SELECT	'lance.garcia@yaspeak.com',		        'english',      'native'		UNION ALL
    SELECT	'sumaiya.beltran@yaspeak.com',	        'english',      'native'		UNION ALL
    SELECT	'delilah.shannon@yaspeak.com',	        'english',      'native'		UNION ALL
    SELECT	'alyssia.owen@yaspeak.com',		        'english',      'native'		UNION ALL
    SELECT	'tyrell.meadows@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'craig.bender@yaspeak.com',		        'english',      'native'		UNION ALL
    SELECT	'mae.ali@yaspeak.com',			        'english',      'native'		UNION ALL
    SELECT	'malakai.cherry@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'krishan.foley@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'denzel.mcclain@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'justin.miller@yaspeak.com',		    'english',      'native'		UNION ALL
    SELECT	'libbie.king@yaspeak.com',		        'english',      'native'
)
INSERT INTO r_user_language(user_id, language_id, language_level_id, is_default)
SELECT u.user_id, l.language_id, ll.language_level_id, CAST(0 AS boolean)
FROM user_language ul
         INNER JOIN app_user u on (ul.login = u.login)
         INNER JOIN c_language l on (ul.language = l.enum_key )
         INNER JOIN c_language_level ll on (ul.level = ll.enum_key);

-- ATTRIBUTES
with attributes (login, attribute_type, text) as (
    SELECT 'alice.anderson@yaspeak.com',          'description',              'Hello! I''m Alice, from Stockholm.  I''ve been traveling and living in different countries since 2008 and am currently settled in Turkey. I am TEFL certified and have been teaching English to adults and teens since 2012. I specialize in business and academic English. This means I help people prepare for job interviews, presentations, and dealing with professional negotiations and discussions; develop their conversation skills, writing, and grammar; and get ready for exams such as IELTS and TOEFL.  I believe practice makes perfect and encourage you to speak as much as possible, then I can help you correct any mistakes and advise you on how to sound more professional and natural in your speaking. I am very patient and am happy to go over any questions you have in as much detail as you need until you feel 100% confident in using English. A trial lesson is a great way to discuss your current level and what you''ll need to focus on going forward. I look forward to seeing you soon!' UNION ALL
    SELECT 'alice.anderson@yaspeak.com',          'video_url',                'https://youtu.be/2GhwhtwGQkU' UNION ALL
    SELECT 'emma.smith@yaspeak.com',              'description',              'Hello, my name is Emma, and I''m from San Jose, California. Currently, I live in Madrid, Spain. I love meeting new people and helping them reach their goals, especially with English! I enjoy taking yoga classes, going on hikes, swimming in the ocean, reading, trying new recipes, and going to concerts with friends.  I have 4 years of experience and I am TEFL-certified. I tutored children, ages 6-13 in Language Arts, Math, and Reading. I assisted with various level classes at an ESL school. I traveled to Santiago, Chile to teach beginner, intermediate, and advanced ESL classes at a University. And now, I am an assistant culture and language (English) teacher at an elementary school in Madrid. My teaching method is to meet students WHERE they are at. Every student is different, therefore lessons need to be flexible and most productive for that student. I always enjoyed the fun, unconventional assignments when I was a student. Those assignments kept me motivated and helped me not only understand but retain the information better! Book a trial lesson with me so we can discuss your goals and how I can help you reach them!' UNION ALL
    SELECT 'emma.smith@yaspeak.com',              'video_url',                'https://youtu.be/YXgeD4GyVHM' UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',      'description',              'Hello! My name is Britney. I am a traveller and a language lover. I am travelling now, so I speak English every day. I can teach you how to overcome your language barrier and communicate without any problems! I have a bachelor''s degree in teaching foreign languages. I have a TEFL&TESOL certificate and 4 years of experience working in a private language school.  I want to offer you conversational lessons with a lot of speaking practice and not much exercise because I want my students to really use the language. No matter how perfect your grammar is you will never be fluent until you have enough speaking practice! However, if you have some difficulties with grammar, pronunciation or anything else, I will help you overcome them! I have taught formal English classes for years, so I know how to explain and practice any aspect of the language! Book a session with me to hear more about how we can work together and get your career moving onwards and upwards.' UNION ALL
    SELECT 'britney.johansberg@yaspeak.com',      'video_url',                'https://youtu.be/4WMdrqbPzzA' UNION ALL
    SELECT 'alisa.orlova@yaspeak.com',            'description',              '💫𝑩𝒆𝒔𝒕 𝒕𝒆𝒂𝒄𝒉𝒆𝒓 𝒐𝒏 Yaspeak 𝒊𝒏 2022 with a premium customizable lesson💫 \n About Me \n yaspeak teacher since Feb 10, 2022 \n I am a passionate traveler who has explored 40+ cities and 20+ countries worldwide. 🌍 My love for learning languages, cultures, and histories has led me to visit fascinating places such as the US, Indonesia, Vietnam, China, Hongkong, Japan, Russia, Spain, France, Germany, Austria, Czech Republic, Philippines, Australia, the UK, Portugal, Malta, Belgium, Italy, Switzerland, Netherlands, Qatar, Malaysia, and Singapore. 🗺️ I am now in Canada! \n I am committed to constantly learning and growing, both personally and professionally. As an educator, I find joy in connecting with students from diverse backgrounds and sharing my love for learning with them. ❤️' UNION ALL
    SELECT 'julia.berko@yaspeak.com',             'description',              'Hello to my new lovely students! Please note that I will be on vacation from April 19 to 30th, all availabilities are up to date for the dates after, I''ll be honored to meet you after 😊  \n Hello :) \n My name is Shay and I''m based in Montreal, Canada. \nI moved to Montreal as an immigrant over 15 years ago. Tackling two brand new languages (French & English) at once was most definitely a challenge, but I wouldn''t have done it any other way. \n Living in this dynamic and diverse community has open my eyes to new perspectives, and I hope to share with the international community my knowledge and my experiences. \n See you soon! À bientôt! Hasta luego! 下次再见！' UNION ALL
    SELECT 'oksana.sonnova@yaspeak.com',          'description',              'My name is Esther, I''m a certified English teacher, and in 2019 I started tutoring French. I''ve lived in different countries (France, Spain, UK, Canada, India, Germany) and I''m passionate about languages. \n\n Je m''appelle Esther, je suis professeur d''anglais certifiée et l''année dernière et en 2019 j''ai commencé à donner des cours particuliers de français. J''ai vécu dans différents pays étrangers (Espagne, Royaume-Uni, Canada, Inde, Allemagne) et suis passionnée de langues.' UNION ALL
    SELECT 'jenny.santana@yaspeak.com',           'description',              '✅5300+ lessons✅Daily topics✅Beginner✅HSK✅Conversational✅The Writer on Chairman''s Bao \n 😃【I am June and .......】 \n \n 👩‍🎓  Lifelong Learner ' UNION ALL
    SELECT 'jerome.coffey@yaspeak.com',           'description',              'My interests: \n🎄Language, culture and tradition \n 🏆Sport \n 🖍️Proofreading and translation  \n 📺 Anime, Disney, tv shows \n ✈️Travels \n 🧐 Intercultural communication \n 💻 Computational linguistics \n 🐻 Cats, dogs and animals \n 📚 Literature \n 📰 Newspapers \n 🈲 Politics, religion, social taboos \n 👿 Dante inferno \n' UNION ALL
    SELECT 'ayden.townsend@yaspeak.com',          'description',              'Hello! I''m an a human beeing, born and raised in South Africa and now living in New Zealand. I love teaching, and it makes me happy to see people improving their English. \n I have also taught myself Hebrew so I can completely understand what it is like to learn a foreign language. It''s taken a lot of dedication and time, but I have experienced a lot of joy and satisfaction in my language learning journey, and that is how it should be.' UNION ALL
    SELECT 'cassie.pope@yaspeak.com',             'description',              'Hello! I am happy to introduce myself. My name is X Y, and I lived the first fifty-four years of my life in New York and Florida where I had a thirty-year career in Corporate Accounting and Finance Management upwards to CFO. In 2017,  I desired change and transitioned into my current TEFL career and a new country. I now live in Cartagena, Colombia (a beautiful coastal city). I was a competitive athlete throughout my life (baseball and basketball) and continue to be an exercise and fitness enthusiast. I have two young children ages 8 and 9 with my wife who is a native Colombian. I also coach baseball, and basketball, and am a personal trainer.' UNION ALL
    SELECT 'jazmin.rodgers@yaspeak.com',          'description',              'Hello, I am a 27 year old Philosophy Ph.D student who lives in Valencia, Spain. I am a  native Spanish speaker and  C2 English speaker, also i speak fluent Portugues and im learning Czech and German. I consider myself an extroverted person, so i try to make my classes fun and comfortable for calm students. Let me tell you a bit more about myself, besides being a student I am also a Jiujitsu lover, a Muay Thai enthusiast and a adventure traveler. I love cooking as much as I love wine and a good book with great company. \n “The beautiful thing about learning is that nobody can take it away from you.” – B.B. King' UNION ALL
    SELECT 'lyra.larsen@yaspeak.com',             'description',              '7K+ done in 3 years. Thanks all. Next goal 10K.\n Hello, my name is Hans Simons. I''m from the Netherlands and have lived in many countries such as the US, the UK, Colombia, Italy and Spain. I love reading, a good movie, cooking, thus good food. As you can see, I have travelled the world, which has given me a lot to talk about.\n\nMijn naam is Hans, en ik ben een verpleegkundige uit Nederland. Heb veel gereisd en heb dus behoorlijk wat ervaring en taal opgedaan gedurende al deze jaren. Mijn grootste hobby''s (naast reizen) zijn lezen, een goede film en goed eten (kook dus zelf.) \nSoy Hans, y hablo bien el español. Después de vivir más de 15 años en América Latina y España, creo que es justo.' UNION ALL
    SELECT 'vera.mcgowan@yaspeak.com',            'description',              'Hi everyone! Thank you for visiting my profile and beginning your English journey with me. \n I love getting to know new people and learning about different cultures! This is one of the reasons I became a teacher, to share my experience and knowledge with people who are looking to broaden their horizons and learn a new language in a fun and friendly way. I have over 6 years teaching experience, teaching children and adults of all ages so you''ll be in safe hands.' UNION ALL
    SELECT 'chad.rivas@yaspeak.com',              'description',              'Hi! I''m Meinard, a TESOL certified Professional Teacher from Davao City, Philippines! Besides teaching, I am a hobbyist photographer. I like to discuss movies, music, Philosophy, and anything under the sun. \n I have students from many different industries: Business Management, HR, Software Development, Hospitality, and the like- with varied language levels. I believe the language level is not an obstacle as long as the learner is determined. So, I hope to have you in my lessons soon!' UNION ALL
    SELECT 'lance.garcia@yaspeak.com',            'description',              'Greetings!  I''m Warren! (he/him)  👋👋  I''m a Communication Coach specialized in helping clients prepare for *interviews*! \n I''m host of the Confident English Interview Podcast, & CEI Coaching, a blog & podcast to help you get ready for your important interview. \n I''ve been working with clients preparing for interviews since 2018.  My clients get results: new jobs, better income, and advancement in their careers. 📈📈 \n If you''re preparing for a job interview, university/masters interview, residency interview, etc - I can help you with the strategy & practice in mock interviews \n Interview coaching for non-native English speakers is my speciality.' UNION ALL
    SELECT 'sumaiya.beltran@yaspeak.com',         'description',              'My name is Hanna and I''m originally from Sweden, but I love traveling and have lived in four different countries other than Sweden. I''m 26 years old and love learning new languages myself, so I can really connect with my own students in their pursuit to learn. \n I taught English in both Vietnam, Ho Chi Minh City and Spain, Barcelona, before moving onto mainly teaching online here on Yaspeak. The teacher training I underwent in both Vietnam and Spain is what I carry with me to make sure my lessons are effective, fun and personalized. \n The biggest lessons I''ve learned through teaching is that everyone is different, and lessons therefore need to be tailored to the individual student.' UNION ALL
    SELECT 'delilah.shannon@yaspeak.com',         'description',              'Hello! My name is Antonia. I''m a native English speaker from Northern Ireland, with a passion for travel, culture, and languages. Having travelled throughout Europe, with plans to explore more, my appreciation and love for communicating with people from all round the world has driven me to learn foreign languages, and spend my time teaching others how to speak English. I can’t wait to share this passion with you!' UNION ALL
    SELECT 'alyssia.owen@yaspeak.com',            'description',              'I care deeply about my students. As an yaspeak professor, I specialize in American English: Levels A1 through C2, Business English, Advanced Conversation for Social and Business purposes, and TOEIC test preparation. I have worked with diverse client-learners from multiple countries - students in various professions and with varied interests, such as business, aviation, auto industry, nursing, finance, travel, American culture, and more.' UNION ALL
    SELECT 'tyrell.meadows@yaspeak.com',          'description',              'I was born and raised in the Philippines. I am a patient, flexible and gregarious teacher.  I am a certified TESOL and TEFL teacher. I''ve taught students of all levels and ages from Asian and European countries for many years. I''ve taught IELTS, General English, Business English, and ESL kids' UNION ALL
    SELECT 'craig.bender@yaspeak.com',            'description',              'Hi, I''m a human (BSc, TEFL) \n \n I''m an experienced English & Technical Teacher and Data Engineer. \n ▶ TEFL Certified \n ▶ British English (Southern Accent)\n ▶ 4+ Years Global Teaching experience with all ages & abilities \n ▶ BSc Mathematics with First Class Honors (4.0 GPA)\n ▶ Data Engineer (Python)\n ▶ Financial Services Background\n ▶ 3+ Years corporate tech experience in multi-national company' UNION ALL
    SELECT 'mae.ali@yaspeak.com',                 'description',              'Languages are the key to success. \n\n I am a human beeing, I come from Barcelona, Spain.\n I hold a Bachelor''s Degree in Languages, Translation and Interpreting from Universitat Autònoma de Barcelona.\n I am really into cooking, learning new languages, reading and dancing.\n I have been an English teacher for over 3 years at a private language school in Spain.' UNION ALL
    SELECT 'malakai.cherry@yaspeak.com',          'description',              '大家好， 我很高兴认识你，我叫LUIS！Welcome to my profile! Hello everyone! my name is LUIS, i am a business advisor living in Asia for over 9 years. As a language tutor in my spare time, I''m here to improve your conversational skills, grammar and make your English🇺🇸/Spanish🇪🇨 language learning a fruitful experience! I enjoy hiking, being the most amazing trekking experience so far a 10-day walk to Mount Everest 珠穆朗玛峰 in 2019. \n \n In March 2023 i was accredited as TESOL/TEFL certified teacher by the World TESOL Academy. \n I offer lessons in:\n ✅ General conversations, One-one topic focus conversations\n ✅ Academic paper revision, homework revision\n ✅ BusinessENG\n ⁉️非常重要的: 第一个-如果你住在中国，我用ZOOM在我们的课程。第二个-对不起，我不教孩子们。' UNION ALL
    SELECT 'krishan.foley@yaspeak.com',           'description',              'I have experience teaching babies, children, people with special needs, disabilities & distracted kids. \n Learning Loss Recovery, Tutoring. Group Classes available. \n I have a Bachelor''s Degree in Elementary Education with an emphasis in Early Childhood Education.  I specialize in teaching kids, babies, toddlers, children of all ages.  TEFL certified with an Advanced Certificate in Teaching English to Young Learners (TEYL).  I have a minor in French and I am learning Spanish. I am an online English teacher, a former classroom teacher, and a homeschool mom.  I have visited France, Mexico, The Netherlands, Canada, Honduras, Romania, and Bahamas.' UNION ALL
    SELECT 'denzel.mcclain@yaspeak.com',          'description',              'An American 🇺🇸 from the state of Maryland (馬里蘭州), I came to Taiwan in 2005 to teach English. My classes are LGBT + / queer friendly 🏳️‍🌈 🏳️‍⚧️, and I teach all ages, nationalities, & learning differences (ADHD, ASD). Fluent in Mandarin 🇹🇼 🇨🇳, I also study French & Spanish. \n\n Experience: \n 6+ years at the Princeton Review. Was certified to teach TOEFL and developed A1-C2 English courses for high school, college, & adult students. \n Since 2014, I''ve coached 100s of international technology startup CEOs & judged countless business competitions globally. I''ve hired many employees in my career, & helped numerous yaspeak students ace job & university interviews.' UNION ALL
    SELECT 'justin.miller@yaspeak.com',           'description',              '🌸Teacher Shaney 🌸 \n Hi there ! 👋🏻 My Name is Shaney and I come from South Africa. I am a Level 5 TEFL qualified teacher. I have worked with many different business professionals in the past and I also have experience in working with kids. I am a passionate teacher and I would love to help you reach your learning goals. One of my favorite quotes says that you don''t have to see the whole staircase, just take the first step. So lets take the first step together and start your English journey! 👩‍🏫' UNION ALL
    SELECT 'libbie.king@yaspeak.com',             'description',              'My name is Jana and I am a Canadian living in Austin, Texas, USA.  I have Bachelor’s degree in Russian Studies & History from Dalhousie University in Halifax, NS, Canada. I hold master’s degree in Slavic Literature and Language from the University of Toronto in Toronto, ON, Canada. I speak French, Russian, and am currently learning Mandarin Chinese. Please note I do not work with children under the age of 18.'
)
INSERT INTO r_user_attribute(user_id, user_attribute_type_id, value)
SELECT u.user_id, ua.user_attribute_type_id, replace(a.text, '\n', E'\n')
FROM attributes a
         INNER JOIN app_user u on (a.login = u.login)
         INNER JOIN c_user_attribute_type ua on (a.attribute_type = ua.enum_key);

-- REVIEWS (todo only student are giving ratnigs)
with reviews (reviewer, reviewee, rate, comment) as (
    SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         3,      'The tutor is great, but she was late twice.'   UNION ALL
    SELECT 'emily.jick@yaspeak.com',      'emma.smith@yaspeak.com',             4,      'She is fantastic and very friendly. She focuses on you and takes notes during the class in order to improve your English productively and efficiently. I definitely suggest everyone get into a class with her.'    UNION ALL
    SELECT 'emily.jick@yaspeak.com',      'britney.johansberg@yaspeak.com',     5,      'Britney is an excellent teacher, she masters all subjects about grammar. I do enjoy taking classes with her. '
)
INSERT INTO app_user_review (reviewer_id, reviewee_id, rate, comment, created, updated)
SELECT reviewer.user_id, reviewee.user_id, r.rate, r.comment, get_UTC_time(), get_UTC_time()
FROM reviews r
         INNER JOIN app_user reviewer on (r.reviewer = reviewer.login)
         INNER JOIN app_user reviewee on (r.reviewee = reviewee.login);

-- CONVERSATIONS
WITH conversations
         (           initiator,              opponent ) as (
        SELECT  'emily.jick@yaspeak.com',  'alice.anderson@yaspeak.com'     UNION ALL
        SELECT  'emily.jick@yaspeak.com',  'emma.smith@yaspeak.com'         UNION ALL
        SELECT  'emily.jick@yaspeak.com',  'britney.johansberg@yaspeak.com'
    )
INSERT INTO app_conversation (initiator_id, opponent_id, created, updated)
SELECT  i.user_id, o.user_id, get_UTC_time(), get_UTC_time()
FROM conversations c
         INNER JOIN app_user i on (c.initiator = i.login)
         INNER JOIN app_user o on (c.opponent = o.login);

-- MESSAGES
WITH messages
         (      sender,                     receiver,                           text_value,            group_id,     id_for_attachment) as (
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 1', 1, CAST(NULL as INTEGER)                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 2, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 3', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 4, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 5', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 6, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 7', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 8, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 9', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 10, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 11', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 12, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 13', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 14, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 15', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 16, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 17', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 18, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 19', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 20, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 21', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 22, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 23', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 24, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 25', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 26, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 27', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 28, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 29', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 30, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 31', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 32, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 33', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 34, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 35', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 36, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 37', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 38, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 39', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 40, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 41', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 42, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 43', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 44, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 45', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 46, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 47', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 48, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 49', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 50, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Lorem ipsum 51', 1, NULL                                                                                                                                                                                                             UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Lorem ipsum 52, but what does it mean? ☺', 1, NULL                                                                                                                                                                                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Hi Alice! I''m Emily from Istambul. I think my language level is someWHERE between B1 and B2, I undersand a lot, but I''m afraid of speking with english native speakers. Could you help me with that?', 2,  NULL                    UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Hi Emily! Yes, sure! Book the trial ☺ and we can try it out!', 2, NULL                                                                                                                                                              UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'That''s amazing! Thank you Alice for you answer', 2, NULL                                                                                                                                                                           UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Sure, no problem', 2, NULL                                                                                                                                                                                                          UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Thank you Alice for the lesson, it was really helpful, can''t wait an another one!', 3, NULL                                                                                                                                        UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Thank you, it was fun. Me too.', 3, NULL                                                                                                                                                                                            UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Hi Alice, I forgot what I need to do for a home work, cau you please write it down? Thank you very much! ', 4, NULL                                                                                                                 UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'Yes'' sure, not a problem. You need to read the text and answer on questions. That''s it :D.', 4, 1                                                                                                                                UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'And also don''t forget to read it out loud',  4, NULL                                                                                                                                                                               UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'alice.anderson@yaspeak.com',         'Thank you Alice very much, you are the best!', 4, NULL                                                                                                                                                                              UNION ALL
        SELECT 'alice.anderson@yaspeak.com',  'emily.jick@yaspeak.com',             'See you at the class Emily!', 4, NULL                                                                                                                                                                                               UNION ALL

        SELECT 'emily.jick@yaspeak.com',      'emma.smith@yaspeak.com',             'Hi Emma! I''m Emily from Czechia. Currently I''m struggling with english grammar. I need to have high score in IELTS, but my grammar is far from perfect. Could you help me with that?', 4, NULL                   UNION ALL
        SELECT 'emily.jick@yaspeak.com',      'britney.johansberg@yaspeak.com',     'Hi Britney! I''m Emily from Slovenia. I think I have B2 in engish, but I want to improve my prononcuation. Could you help me with that?', 4, NULL
    )
SELECT row_number() over () as id, x.sender, x.receiver, x.text_value, x.id_for_attachment, group_id
INTO tmp_messages
FROM (
         SELECT
                     row_number() over () as id, sender, receiver, text_value, id_for_attachment, group_id
         FROM messages
         ORDER BY 1 DESC
     ) x;

WITH attachments (attachment_name) as (
    SELECT 'english_text.pdf' UNION ALL
    SELECT 'english_text_2.pdf' UNION ALL
    SELECT 'my_name_is_john.pdf' UNION ALL
    SELECT 'our_vacation.pdf'
)
SELECT * INTO tmp_attachments FROM attachments;

INSERT INTO app_message (   conversation_id,    sender_id,  receiver_id,    message_state_id,       text_value,     created,            updated)
SELECT x.*, x.created as updated
FROM (
         SELECT                      c.conversation_id,  s.user_id,  r.user_id,      1 as message_state_id,  m.text_value,   get_UTC_time() - m.id * interval '1 minute' - (m2.max_group_id - m.group_id) * interval '1 day' as created
         FROM tmp_messages m
                  CROSS JOIN (SELECT MAX(group_id) as max_group_id FROM tmp_messages) m2
                  INNER JOIN app_user s on (m.sender = s.login)
                  INNER JOIN app_user r on (m.receiver = r.login)
                  INNER JOIN app_conversation c on ((c.initiator_id = s.user_id AND c.opponent_id = r.user_id) or (c.initiator_id = r.user_id AND c.opponent_id = s.user_id))
     ) x
order by x.created asc;

INSERT INTO app_message_attachment (name, message_id, content_type, file_path)
SELECT a.attachment_name, m2.message_id, 'application/pdf', concat('message/', m2.message_id, '/', a.attachment_name)
FROM tmp_messages m
         INNER JOIN app_message m2 on m.text_value = m2.text_value
         CROSS JOIN tmp_attachments a
WHERE m.id_for_attachment IS NOT NULL;

insert into sf.app_file
(unique_key,                                    content_type,       path,                                                           file_name,                  environment,    creator_id, updater_id, created,            updated,            state_id, compression_type_id)
SELECT a.attachment_name,                       'application/pdf',  concat('message/', m2.message_id, '/', a.attachment_name),      a.attachment_name,          'ulearn-dev',   u.user_id,  u.user_id,  get_UTC_time(),     get_UTC_time(),     10,        1
FROM tmp_messages m
         INNER JOIN app_message m2 on (m.text_value = m2.text_value)
         INNER JOIN app_user u on (m.sender = u.login)
         CROSS JOIN tmp_attachments a
WHERE m.id_for_attachment IS NOT NULL;

DROP TABLE tmp_messages;

-- RESERVATION PACKAGES (UTC time)
with reservation_packages
         (          reservation_package_id,  state_id,   host,                             attender,                  created,                                                       total_price, price_without_sales) as (
        SELECT 1,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '50 days 20hours',     2700.0,     3000.0      UNION ALL
        SELECT 2,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '49 days 20hours',     2700.0,     3000.0      UNION ALL
        SELECT 3,                       2,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '48 days 20hours',     2700.0,     3000.0      UNION ALL
        SELECT 4,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '47 days 20hours',     2700.0,     3000.0      UNION ALL
        SELECT 5,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '46 days 20hours',     2700.0,     3000.0      UNION ALL
        SELECT 6,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '45 days 20hours',     300.0,      300.0       UNION ALL
        SELECT 7,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '44 days 20hours',     300.0,      300.0       UNION ALL
        SELECT 8,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '43 days 20hours',     300.0,      300.0       UNION ALL
        SELECT 9,                       1,          'alice.anderson@yaspeak.com',      'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '42 days 20hours',     300.0,      300.0       UNION ALL
        SELECT 10,                      1,          'emma.smith@yaspeak.com',           'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '42 days 10hours',     300.0,      300.0       UNION ALL
        SELECT 11,                      1,          'britney.johansberg@yaspeak.com',   'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '41 days 20hours',     500.0,      500.0       UNION ALL
        SELECT 12,                      1,          'britney.johansberg@yaspeak.com',  'emily.jick@yaspeak.com',     get_truncated_week_PRG_time() - INTERVAL '40 days 20hours',     700.0,      700.0
    )
SELECT * INTO tmp_reservation_packages FROM reservation_packages;

INSERT INTO app_reservation_package
(       host_id,        attender_id,    created,    cancelled,      cancellation_reason,    reservation_package_state_id,   total_price,    currency_id,        class_duration_id)
SELECT
    h.user_id,      a.user_id,      r.created,  null,           null,                   r.state_id,                     total_price,    1,                  2
FROM tmp_reservation_packages r
         INNER JOIN app_user h on (r.host = h.login)
         INNER JOIN app_user a on (r.attender = a.login);

-- RESERVATIONS (UTC time)
with reservations
         (      subject,    host,                           attender,               price,  price_with_sales,   state_id,       package_id,     start_time,                                                     room_key                                                    ) as (
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '50 days 10hours',     'hqbj-auvi-fy6o'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '49 days 10hours',     '9iog-6k3o-5dhm'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '48 days 10hours',     'dkv4-jkpz-y1e5'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '47 days 10hours',     'usaq-zmka-q3eu'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '46 days 10hours',     'hqb1-hjj2-lx7a'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '45 days 10hours',     'h9qk-o00l-cvy2'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '44 days 10hours',     'xgxd-w66x-ck7f'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '43 days 10hours',     'ynfq-11br-d5a8'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '42 days 10hours',     '4hxh-fo4z-otjg'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              1,              get_truncated_week_PRG_time() - INTERVAL '41 days 10hours',     'lifm-eoqb-8azh'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '40 days 10hours',     'xv9r-bi2x-0rfp'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '39 days 10hours',     'kip6-6xvl-61wr'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '38 days 10hours',     '5vlu-qz9s-jbtz'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '37 days 10hours',     '9yd2-mx6l-fr8a'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '36 days 10hours',     'xdfq-lv0u-3973'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '35 days 10hours',     '2gnj-qkxj-sg3n'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '34 days 10hours',     'qbny-pbb3-qa7r'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '33 days 10hours',     'lnw8-7lpa-mc0k'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '32 days 10hours',     'yokv-mz13-1ywn'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              2,              get_truncated_week_PRG_time() - INTERVAL '31 days 10hours',     'jwzx-5hri-hs9b'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              3,              get_truncated_week_PRG_time() - INTERVAL '30 days 10hours',     'v032-mtfd-njfk'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '29 days 10hours',     'kk12-gr4u-0t2j'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '28 days 10hours',     'gcp7-d2k1-v568'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '27 days 10hours',     'v8ro-hgjn-2uif'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '26 days 10hours',     'p3uu-wxex-blhm'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '25 days 10hours',     'q3sz-7vqq-jldk'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '24 days 10hours',     'qyom-7yim-nsvg'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '23 days 10hours',     '4v68-6rjy-xxww'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '22 days 10hours',     'ezn6-jobd-or4l'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              3,              get_truncated_week_PRG_time() - INTERVAL '21 days 10hours',     'zi5l-hy30-qth6'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '20 days 10hours',     'n1l7-9m3f-bw1n'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '19 days 10hours',     '6k8o-mysj-yeqd'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '18 days 10hours',     'ctfv-dtv8-l66h'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '17 days 10hours',     '03l9-staa-vd47'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '16 days 10hours',     'ow2q-yj0j-kioa'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '15 days 10hours',     'oqul-zdkb-m1i9'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '14 days 10hours',     '74ok-sxl9-5q9u'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              4,              get_truncated_week_PRG_time() - INTERVAL '13 days 10hours',     'fu5c-m6f4-hpe5'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                9,              4,              get_truncated_week_PRG_time() - INTERVAL '12 days 10hours',     'u6wq-1roj-451t'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                7,              4,              get_truncated_week_PRG_time() - INTERVAL '11 days 10hours',     'x7ge-du00-j2cq'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '10 days 10hours',     'wwwn-06oc-neb5'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '9 days 10hours',      'yrg8-ozly-i4qg'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '8 days 10hours',      'mx72-vz1s-qcdp'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '7 days 10hours',      'pndy-2zbf-ba17'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '6 days 10hours',      '5t8s-982t-c7j4'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '5 days 10hours',      'n2l3-nrh6-xvhm'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '4 days 10hours',      'udxx-jmjy-bzrx'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '3 days 10hours',      '2qpt-msr0-lu4h'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '2 days 10hours',      '9fjh-zfdp-kutu'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              5,              get_truncated_week_PRG_time() - INTERVAL '1 days 10hours',      '6inx-1ic0-vzto'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                2,              6,              get_truncated_day_PRG_time() - INTERVAL '0 days 7hours',        'vcu2-mnts-ebhx'        UNION ALL
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    270,                4,              7,              get_truncated_day_PRG_time() - INTERVAL '0 days 3hours',        'vcu2-mnts-ebhx'        UNION ALL

        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    300,                4,              8,              get_truncated_week_PRG_time() + INTERVAL '7days 7hours',        'yevy-8d47-84xj'        UNION ALL   -- alice.andreson       monday      7:00    -   8:00
        SELECT 'english',   'alice.anderson@yaspeak.com',     'emily.jick@yaspeak.com', 300,    300,                4,              9,              get_truncated_week_PRG_time() + INTERVAL '8days 10hours',       'vg2w-nhsv-727l'        UNION ALL   -- alice.andreson       tuesday     10:00   -   11:00
        SELECT 'english',   'emma.smith@yaspeak.com',         'emily.jick@yaspeak.com', 500,    500,                4,              10,             get_truncated_week_PRG_time() + INTERVAL '9days 8hours',        'gw5a-ljnz-l3dc'        UNION ALL   -- emma.smith           wednesday   8:00    -   9:00
        SELECT 'english',   'britney.johansberg@yaspeak.com', 'emily.jick@yaspeak.com', 700,    700,                4,              11,             get_truncated_week_PRG_time() + INTERVAL '9days 7.5hours',      '8380-vto1-qfl6'        UNION ALL   -- britney.johansberg   wednesday   7:30    -   8:30
        SELECT 'english',   'britney.johansberg@yaspeak.com', 'emily.jick@yaspeak.com', 700,    700,                4,              12,             get_truncated_week_PRG_time() + INTERVAL '10days 9.5hours',     'sve9-hcfr-9yae'                    -- britney.johansberg   thursday    9:30    -   10:30
    )
SELECT * INTO tmp_reservations FROM reservations;

-- ROOMS
INSERT INTO app_room
(       room_key,           room_state_id, host_id,     attender_id,    scheduled_from_time,      scheduled_to_time)
SELECT  room_key,           1,             h.user_id,   a.user_id,      r.start_time,             r.start_time + INTERVAL '1hour'
FROM tmp_reservations r
         INNER JOIN app_user h on (r.host = h.login)
         INNER JOIN app_user a on (r.attender = a.login);

INSERT INTO app_reservation (reservation_package_id,    subject_id,    host_id,    attender_id,    reservation_state_id,    price,               price_without_sales,            created,           updated,            start_time,       end_time,                             expected_end_time,                              room_id,          host_event_iana_timezone,     currency_id,    class_duration_id)
SELECT                      r.package_id,              s.subject_id,   h.user_id,  a.user_id,      r.state_id,              r.price_with_sales,  r.price,                        p.created,         p.created,          r.start_time,     r.start_time + INTERVAL '1hour',      r.start_time + INTERVAL '50 minutes',           room.room_id,     'Europe/Prague',              1,              2
FROM tmp_reservations r
         INNER JOIN app_user h on (r.host = h.login)
         INNER JOIN app_user a on (r.attender = a.login)
         INNER JOIN c_subject s on (r.subject = s.enum_key)
         INNER JOIN app_reservation_package p on (p.reservation_package_id = r.package_id)
         INNER JOIN app_room room on (h.user_id = room.host_id AND a.user_id = room.attender_id AND r.start_time = room.scheduled_from_time);

-- rescheduled reservation
INSERT INTO app_reservation
(   source_reservation_id,  reservation_package_id,     subject_id,      host_id,    attender_id,       reservation_state_id,               price,                  price_without_sales,            created,                            updated,                            start_time,                           end_time,                             expected_end_time,                              room_id,            host_event_iana_timezone,   currency_id,                class_duration_id)
SELECT
    r.reservation_id,       r.reservation_package_id,   r.subject_id,    r.host_id,  r.attender_id,     r.reservation_state_id,             r.price,                r.price_without_sales,          r.created + INTERVAL '1 hour',      r.created + INTERVAL '1 hour',      r.start_time + INTERVAL '1 hour',     r.start_time + INTERVAL '2 hours',   r.start_time + INTERVAL '1 hour 50 minutes',     r.room_id,          r.host_event_iana_timezone,            r.currency_id,   1
FROM app_reservation r
         INNER JOIN app_user h on (r.host_id = h.user_id)
         INNER JOIN app_user a on (r.attender_id = a.user_id)
WHERE h.login = 'alice.anderson@yaspeak.com' AND a.login = 'emily.jick@yaspeak.com' AND r.reservation_state_id = 9;


INSERT INTO app_reservation_issue
(           reservation_issue_type_id,  reservation_id,     reservation_issue_state_id, description,                                created,                            resolved,                       creator_id,     resolver_id, decision_text )
    (SELECT      2,                          reservation_id,     2,                          'She was absent the whole lesson',          end_time + INTERVAL '1day 1 hour',  end_time + INTERVAL '2days',    attender_id,    1,          'Your''re right, we returned you money for the lesson. We apoligize for that and make sure it won''t happen again.'
     FROM app_reservation
     WHERE reservation_state_id = 7
    )
UNION ALL
(SELECT      2,                          reservation_id,     3,                          'She was absent for the second time...',    end_time + INTERVAL '1day 2 hour',  end_time + INTERVAL '2days',    attender_id,    1,          'We''ve checked the attendence and in fact your tutor wasn''t absent. '
 FROM app_reservation ORDER BY reservation_id LIMIT 1 OFFSET 1);


with transactions
         (       vendor_id,  customer_id,    tmp_group_id,                               user_wallet_id,    transaction_type_id,     amount,                                                                             created,                                                transaction_state_id,   payment_service_provider_id,    reservation_package_id,     creator_id,     reservation_issue_id,               session_id, updated,                                                updater_id, reservation_id,                 bank_account_number,    bank_code,      currency_id,    transaction_refund_reason_id,       planned_completion_time,                                receipt_url)
         AS
         (
             -- deposit money
             SELECT
                 1,          u.user_id,      CONCAT('D-',w.user_wallet_id),              w.user_wallet_id,   1,                      50000.0,                                                                            get_truncated_week_PRG_time() - INTERVAL '51 days',     2,                      1,                              null,                       u.user_id,      CAST(null AS INT),                   null,       get_truncated_week_PRG_time() - INTERVAL '51 days',     u.user_id,  CAST(null AS INT),             null,                   null,           1,              CAST(null AS INT),                  get_truncated_week_PRG_time() - INTERVAL '51 days',     'https://pay.stripe.com/receipts/payment/CAcaFwoVYWNjdF8xTjI3bE1DYm9sOG41WlVWKKCG9KUGMgZiw8Eb_AQ6LBZmS7w-xV6ZxIqDs-W5vCqFqPVlxJYECcc0N3sVzhtrGgQP6Fmp_TTjsgRZ'
             FROM app_user_wallet w
                      INNER JOIN app_user u ON (w.user_id = u.user_id AND u.user_role_id IN (3, 4))
             UNION ALL
             -- package purchase
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('R-',p.reservation_id),              ow.user_wallet_id,  5,                      -p.price,                                                                           p.created,                                              2,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.end_time + INTERVAL '7 days',                         NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.attender_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id in (4, 6)
             UNION ALL
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('R-',p.reservation_id),              ow.user_wallet_id,  5,                      p.price,                                                                            p.created,                                              6,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.end_time + INTERVAL '7 days',                         NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id in (4, 6)
             UNION ALL
             -- lesson commission
             SELECT
                 1,          p.host_id,      CONCAT('CO-',p.reservation_id),              ow.user_wallet_id,  6,                     -0.21*p.price,                                                                      p.created,                                              6,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.end_time + INTERVAL '7 days',                         NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id in (4, 6)
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('CO-',p.reservation_id),              ow.user_wallet_id,  6,                     0.21*p.price,                                                                       p.created,                                              6,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.end_time + INTERVAL '7 days',                         NULL
             FROM app_reservation p
                      INNER JOIN app_user o on o.user_id = 1
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id in (4, 6)
             UNION ALL
             -- package cancellation    
             SELECT
                 1,          p.host_id,      CONCAT('CO-',p.reservation_id),              ow.user_wallet_id,  6,                     -0.21*p.price,                                                                      p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 2
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('CO-',p.reservation_id),              ow.user_wallet_id,  6,                     0.21*p.price,                                                                       p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on o.user_id = 1
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 2
             UNION ALL
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('R-',p.reservation_id),              ow.user_wallet_id,  5,                      -p.price,                                                                           p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.attender_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 2
             UNION ALL
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('R-',p.reservation_id),              ow.user_wallet_id,  5,                      p.price,                                                                            p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 2
             UNION ALL
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('RCM-',p.reservation_package_id),    ow.user_wallet_id,  2,                      p.total_price - (r.price*r.count) - ((r.price_without_sales-r.price) * r.count),    p.created,                                              2,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  CAST(null AS INT),             null,                   null,          1,                1,                                 p.created,                                              NULL
             FROM app_reservation_package p
                      INNER JOIN app_user o on p.attender_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
                      LEFT JOIN (
                 SELECT COUNT(*) AS count, MAX(price_without_sales) as price_without_sales, MAX(price) as price, reservation_package_id
                 FROM app_reservation
                 WHERE reservation_state_id = 4
                 GROUP BY reservation_package_id
             ) r ON r.reservation_package_id = p.reservation_package_id
             WHERE p.reservation_package_state_id = 2
             UNION ALL
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('RCM-',p.reservation_package_id),    ow.user_wallet_id,  2,                      -p.total_price + (r.price*r.count) + ((r.price_without_sales-r.price) * r.count),   p.created,                                              2,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  CAST(null AS INT),             null,                   null,          1,                1,                                 p.created,                                              NULL
             FROM app_reservation_package p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
                      LEFT JOIN (
                 SELECT COUNT(*) AS count, MAX(price_without_sales) as price_without_sales, MAX(price) as price, reservation_package_id
                 FROM app_reservation
                 WHERE reservation_state_id = 4
                 GROUP BY reservation_package_id
             ) r ON r.reservation_package_id = p.reservation_package_id
             WHERE p.reservation_package_state_id = 2
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('PSCC-',p.reservation_package_id),    ow.user_wallet_id,  10,                    -0.21 * (r.price_without_sales-r.price) * r.count,                                  p.created,                                              2,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  CAST(null AS INT),             null,                   null,          1,                CAST(null AS INT),                 p.created,                                              NULL
             FROM app_reservation_package p
                      INNER JOIN app_user o on p.attender_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
                      INNER JOIN (
                 SELECT COUNT(*) AS count, MAX(price_without_sales) as price_without_sales, MAX(price) as price, reservation_package_id
                 FROM app_reservation
                 WHERE reservation_state_id = 4
                 GROUP BY reservation_package_id
             ) r ON r.reservation_package_id = p.reservation_package_id
             WHERE p.reservation_package_state_id = 2
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('PSCC-',p.reservation_package_id),    ow.user_wallet_id,  10,                    0.21 *  (r.price_without_sales-r.price) * r.count,                                  p.created,                                              2,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  CAST(null AS INT),             null,                   null,          1,                CAST(null AS INT),                 p.created,                                              NULL
             FROM app_reservation_package p
                      INNER JOIN app_user o on o.user_id = 1
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
                      INNER JOIN (
                 SELECT COUNT(*) AS count, MAX(price_without_sales) as price_without_sales, MAX(price) as price, reservation_package_id
                 FROM app_reservation
                 WHERE reservation_state_id = 4
                 GROUP BY reservation_package_id
             ) r ON r.reservation_package_id = p.reservation_package_id
             WHERE p.reservation_package_state_id = 2
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('RLC-',p.reservation_package_id),    ow.user_wallet_id,  7,                      0.21*(p.total_price - COALESCE(r.price, 0) * r.count),                             p.created,                                              2,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  CAST(null AS INT),             null,                   null,          1,                1,                                  p.created,                                              NULL
             FROM app_reservation_package p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
                      LEFT JOIN (
                 SELECT COUNT(*) AS count, MAX(price_without_sales) as price_without_sales, MAX(price) as price, reservation_package_id
                 FROM app_reservation
                 WHERE reservation_state_id = 4
                 GROUP BY reservation_package_id
             ) r ON r.reservation_package_id = p.reservation_package_id
             WHERE p.reservation_package_state_id = 2
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('RLC-',p.reservation_package_id),    ow.user_wallet_id,  7,                      -0.21*(p.total_price - COALESCE(r.price, 0) * r.count),                            p.created,                                              2,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  CAST(null AS INT),             null,                   null,          1,                1,                                  p.created,                                              NULL
             FROM app_reservation_package p
                      INNER JOIN app_user o on o.user_id = 1
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
                      LEFT JOIN (
                 SELECT COUNT(*) AS count, MAX(price_without_sales) as price_without_sales, MAX(price) as price, reservation_package_id
                 FROM app_reservation
                 WHERE reservation_state_id = 4
                 GROUP BY reservation_package_id
             ) r ON r.reservation_package_id = p.reservation_package_id
             WHERE p.reservation_package_state_id = 2
             UNION ALL
             -- refund transactions
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('R-',p.reservation_id),              ow.user_wallet_id,  5,                      -p.price,                                                                           p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.attender_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 7
             UNION ALL
             SELECT
                 p.host_id,  p.attender_id,  CONCAT('R-',p.reservation_id),              ow.user_wallet_id,  5,                      p.price,                                                                            p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 7
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('CO-',p.reservation_id),              ow.user_wallet_id,  6,                     -0.21*p.price,                                                                      p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on p.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 7
             UNION ALL
             SELECT
                 1,          p.host_id,      CONCAT('CO-',p.reservation_id),              ow.user_wallet_id,  6,                     0.21*p.price,                                                                       p.created,                                              3,                      null,                           p.reservation_package_id,   o.user_id,      CAST(null AS INT),                   null,       p.created,                                              o.user_id,  p.reservation_id,             null,                   null,          1,                CAST(null AS INT),                  p.created,                                              NULL
             FROM app_reservation p
                      INNER JOIN app_user o on o.user_id = 1
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE p.reservation_state_id = 7
             UNION ALL
             SELECT
                 r.host_id,  r.attender_id,  CONCAT('RI-',r.reservation_id),             ow.user_wallet_id,  4,                      r.price,                                                                            r.created,                                              2,                      null,                           r.reservation_package_id,   o.user_id,      i.reservation_issue_id,              null,       r.created,                                              o.user_id,  r.reservation_id,             null,                   null,          1,                4,                                  r.created,                                              NULL
             FROM app_reservation_issue i
                      INNER JOIN app_reservation r on i.reservation_id = r.reservation_id
                      INNER JOIN app_user o on r.attender_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE i.reservation_issue_state_id = 2
             UNION ALL
             SELECT
                 r.host_id,  r.attender_id,  CONCAT('RI-',r.reservation_id),             ow.user_wallet_id,  4,                      -r.price,                                                                           r.created,                                              2,                      null,                           r.reservation_package_id,   o.user_id,      i.reservation_issue_id,              null,       r.created,                                              o.user_id,  r.reservation_id,             null,                   null,          1,                4,                                  r.created,                                              NULL
             FROM app_reservation_issue i
                      INNER JOIN app_reservation r on i.reservation_id = r.reservation_id
                      INNER JOIN app_user o on r.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE i.reservation_issue_state_id = 2
             UNION ALL
             SELECT
                 1,          r.host_id,      CONCAT('RLC-',r.reservation_id),             ow.user_wallet_id,  7,                    0.21*r.price,                                                                       r.created,                                              2,                      null,                            r.reservation_package_id,   o.user_id,      i.reservation_issue_id,              null,       r.created,                                              o.user_id,  r.reservation_id,             null,                   null,          1,                4,                                   r.created,                                              NULL
             FROM app_reservation_issue i
                      INNER JOIN app_reservation r on i.reservation_id = r.reservation_id
                      INNER JOIN app_user o on r.host_id = o.user_id
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE i.reservation_issue_state_id = 2
             UNION ALL
             SELECT
                 1,          r.host_id,      CONCAT('RLC-',r.reservation_id),             ow.user_wallet_id,  7,                    -0.21*r.price,                                                                      r.created,                                              2,                      null,                            r.reservation_package_id,   o.user_id,      i.reservation_issue_id,              null,       r.created,                                              o.user_id,  r.reservation_id,             null,                   null,          1,                4,                                   r.created,                                              NULL
             FROM app_reservation_issue i
                      INNER JOIN app_reservation r on i.reservation_id = r.reservation_id
                      INNER JOIN app_user o on o.user_id = 1
                      INNER JOIN app_user_wallet ow on o.user_id = ow.user_id
             WHERE i.reservation_issue_state_id = 2
             UNION ALL
             -- money withdrawals
             SELECT
                 1,          u.user_id,      CONCAT('W-',w.user_wallet_id),              w.user_wallet_id,   3,                      -5000.0,                                                                            get_PRG_time() + INTERVAL '3 minutes',                   2,                      1,                              null,                       u.user_id,      CAST(null AS INT),                   null,       get_truncated_week_PRG_time(),                            u.user_id,  CAST(null AS INT),             null,                   null,           1,           CAST(null AS INT),                  get_PRG_time() + INTERVAL '3 minutes',                  NULL
             FROM app_user_wallet w
                      INNER JOIN app_user u ON (w.user_id = u.user_id AND u.login = 'alice.anderson@yaspeak.com')
         )
SELECT * INTO tmp_transactions FROM transactions;

SELECT * INTO tmp_transaction_groups FROM (
                                              SELECT tmp_group_id, ROW_NUMBER () OVER (ORDER BY tmp_group_id) AS id
                                              FROM (
                                                       SELECT DISTINCT tmp_group_id FROM tmp_transactions
                                                   ) t
                                          ) t2;

INSERT INTO app_transaction_group (created)
SELECT get_UTC_time() FROM tmp_transaction_groups;

INSERT INTO app_transaction
(   vendor_id,      customer_id,        transaction_group_id,   user_wallet_id,    transaction_type_id,        amount,                 created,         planned_completion_time,        transaction_state_id,   payment_service_provider_id,    reservation_package_id,     creator_id,     reservation_issue_id,               session_id,     updated,                                                updater_id,     reservation_id,                 bank_account_number,    bank_code,      currency_id,    receipt_url,     transaction_refund_reason_id)
SELECT
    t.vendor_id,    t.customer_id,      g.id,                   user_wallet_id,    transaction_type_id,        amount,                 created,         planned_completion_time,        transaction_state_id,   payment_service_provider_id,    reservation_package_id,     creator_id,     reservation_issue_id,               session_id,     updated,                                                updater_id,     reservation_id,                 bank_account_number,    bank_code,      currency_id,    receipt_url,     transaction_refund_reason_id
FROM tmp_transactions t
         INNER JOIN tmp_transaction_groups g ON t.tmp_group_id = g.tmp_group_id
ORDER BY t.created, t.reservation_id, t.transaction_type_id, t.tmp_group_id;

-- stripe fees (technical wallet)
INSERT INTO app_transaction
(   vendor_id,      customer_id,        transaction_group_id,   user_wallet_id,    transaction_type_id,        amount,                 created,         planned_completion_time,        transaction_state_id,   payment_service_provider_id,    reservation_package_id,     creator_id,     reservation_issue_id,               session_id,     updated,                                                updater_id,     reservation_id,                 bank_account_number,    bank_code,      currency_id,    receipt_url,    source_transaction_id)
SELECT
    1,              1,                  transaction_group_id,   1,                  8,                         -amount * 0.03,         created,         planned_completion_time,        2,                      payment_service_provider_id,    reservation_package_id,     creator_id,     reservation_issue_id,               session_id,     updated,                                                updater_id,     reservation_id,                 bank_account_number,    bank_code,      currency_id,    'https://pay.stripe.com/receipts/payment/CAcaFwoVYWNjdF8xTjI3bE1DYm9sOG41WlVWKKCG9KUGMgZiw8Eb_AQ6LBZmS7w-xV6ZxIqDs-W5vCqFqPVlxJYECcc0N3sVzhtrGgQP6Fmp_TTjsgRZ', transaction_id
FROM app_transaction t
WHERE t.transaction_type_id = 1;

-- set source_transaction_id 
UPDATE app_transaction t
SET source_transaction_id = s.transaction_id
FROM app_transaction s
WHERE t.transaction_type_id = 4 AND t.reservation_id = s.reservation_id AND s.transaction_type_id = 5 AND t.user_wallet_id = s.user_wallet_id;

UPDATE app_transaction t
SET source_transaction_id = s.transaction_id
FROM app_transaction s
WHERE t.transaction_type_id = 7 AND t.reservation_id = s.reservation_id AND s.transaction_type_id = 6 AND t.user_wallet_id = s.user_wallet_id;

-- update wallet statistics
UPDATE app_user_wallet w
SET amount = c.amount
FROM (
         SELECT tw.user_id, SUM(t.amount) as amount
         FROM app_transaction t
                  INNER JOIN app_user_wallet tw on t.user_wallet_id = tw.user_wallet_id
         WHERE t.transaction_state_id in (2, 5)
         GROUP BY tw.user_id
     ) c
WHERE w.user_id = c.user_id;

DROP TABLE IF EXISTS tmp_reservations;
DROP TABLE IF EXISTS tmp_reservation_packages;

INSERT INTO app_recurrent_timeslot(recurrent_timeslot_id, owner_id, local_iso_weekday, from_local_hour, from_local_minute, duration_in_minutes, iana_timezone)
-- "Alice Anderson" | "Mon", "Tue", "Wed" | 7:00 - 21:00
SELECT 1,   user_id,     1,     7,  0,  840,    'Europe/Prague' FROM app_user where login = 'alice.anderson@yaspeak.com' UNION ALL
SELECT 2,   user_id,     2,     7,  0,  840,    'Europe/Prague' FROM app_user where login = 'alice.anderson@yaspeak.com' UNION ALL
SELECT 3,   user_id,     3,     7,  0,  840,    'Europe/Prague' FROM app_user where login = 'alice.anderson@yaspeak.com' UNION ALL
-- "Emma Smith" | "Wed", "Thu", "Fri", "Sat" | 8:00 - 16:00
SELECT 4,   user_id,     3,     8,  0,  480,    'Europe/Prague' FROM app_user where login = 'emma.smith@yaspeak.com' UNION ALL
SELECT 5,   user_id,     4,     8,  0,  480,    'Europe/Prague' FROM app_user where login = 'emma.smith@yaspeak.com' UNION ALL
SELECT 6,   user_id,     5,     8,  0,  480,    'Europe/Prague' FROM app_user where login = 'emma.smith@yaspeak.com' UNION ALL
SELECT 7,   user_id,     6,     8,  0,  480,    'Europe/Prague' FROM app_user where login = 'emma.smith@yaspeak.com' UNION ALL
-- "Britney Johansberg" | "Mon", "Tue", "Wed", "Thu", "Fri" | 7:30 - 12:00
SELECT 8,   user_id,     1,     7,  30, 270,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 9,   user_id,     2,     7,  30, 270,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 10,  user_id,     3,     7,  30, 270,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 11,  user_id,     4,     7,  30, 270,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 12,  user_id,     5,     7,  30, 270,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
-- "Britney Johansberg" | "Mon", "Tue", "Wed", "Thu", "Fri" | 14:00 - 21:00
SELECT 13,  user_id,     1,     14, 0,  420,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 14,  user_id,     2,     14, 0,  420,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 15,  user_id,     3,     14, 0,  420,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 16,  user_id,     4,     14, 0,  420,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com' UNION ALL
SELECT 17,  user_id,     5,     14, 0,  420,    'Europe/Prague' FROM app_user where login = 'britney.johansberg@yaspeak.com';

--reset sequence id
select setval(pg_get_serial_sequence('app_recurrent_timeslot', 'recurrent_timeslot_id'),
              (select max(recurrent_timeslot_id) from app_recurrent_timeslot)
           );

with timeslot_blacklist(recurrent_timeslot_id, local_time) as (
    -- Due to the following reservation: "alice.andreson       monday      7:00    -   8:00"
    SELECT 1, get_truncated_week_PRG_time() + INTERVAL '7days' UNION ALL
    -- Due to the following reservation: "alice.andreson       tuesday     10:00   -   11:00"
    SELECT 2, get_truncated_week_PRG_time() + INTERVAL '8days' UNION ALL
    -- Due to the following reservation: "emma.smith           wednesday   8:00    -   9:00"
    SELECT 4, get_truncated_week_PRG_time() + INTERVAL '9days' UNION ALL
    -- Due to the following reservation: "britney.johansberg   wednesday   7:30    -   8:30" 
    SELECT 10, get_truncated_week_PRG_time() + INTERVAL '9days' UNION ALL
    -- Due to the following reservation: "britney.johansberg   thursday    9:30    -   10:30"
    SELECT 14, get_truncated_week_PRG_time() + INTERVAL '10days'
)
INSERT INTO app_recurrent_timeslot_blacklist(recurrent_timeslot_id, local_year, local_month, local_day)
SELECT t.recurrent_timeslot_id, EXTRACT('Year' FROM local_time), EXTRACT('Month' FROM local_time), EXTRACT('Day' FROM local_time)
FROM timeslot_blacklist t;

with timeslots(timeslot_owner, from_time, to_time) as (
    -- "Alice Anderson" | "Sun" | 12:00 - 14:30
    SELECT 'alice.anderson@yaspeak.com',     get_truncated_week_PRG_time() + INTERVAL '1 week 6 days 12 hours', get_truncated_week_PRG_time() + INTERVAL '1 week 6 days 14 hours 30 minutes' UNION ALL
    -- "Emma Smith" | "Sun" | 12:00 - 14:30
    SELECT 'emma.smith@yaspeak.com',         get_truncated_week_PRG_time() + INTERVAL '1 week 6 days 12 hours', get_truncated_week_PRG_time() + INTERVAL '1 week 6 days 14 hours 30 minutes' UNION ALL
    -- "Britney Johansberg" | "Sun" | 12:00 - 14:30
    SELECT 'britney.johansberg@yaspeak.com', get_truncated_week_PRG_time() + INTERVAL '1 week 6 days 12 hours', get_truncated_week_PRG_time() + INTERVAL '1 week 6 days 14 hours 30 minutes' UNION ALL
    -- reservation:         "alice.andreson       monday      7:00    -   8:00" 
    -- repeating timeslot  -- "Alice Anderson" | "Mon" | 7:00 - 21:00
    SELECT 'alice.anderson@yaspeak.com',     get_truncated_week_PRG_time() + INTERVAL '7days 8hours', get_truncated_week_PRG_time() + INTERVAL '7days 21hours' UNION ALL
    -- reservation:         "alice.andreson       tuesday     10:00   -   11:00"
    -- repeating timeslot  -- "Alice Anderson" | "Tue" | 7:00 - 21:00
    SELECT 'alice.anderson@yaspeak.com',     get_truncated_week_PRG_time() + INTERVAL '8days 7hours', get_truncated_week_PRG_time() + INTERVAL '8days 10hours'  UNION ALL
    -- reservation:         "alice.andreson       tuesday     10:00   -   11:00"
    -- repeating timeslot  -- "Alice Anderson" | "Tue" | 7:00 - 21:00
    SELECT 'alice.anderson@yaspeak.com',     get_truncated_week_PRG_time() + INTERVAL '8days 11hours', get_truncated_week_PRG_time() + INTERVAL '8days 21hours' UNION ALL
    -- reservation:         "emma.smith           wednesday   8:00    -   9:00" 
    -- repeating timeslot  -- "Emma Smith" | "Wed", "Thu", "Fri", "Sat" | 8:00 - 16:00
    SELECT 'emma.smith@yaspeak.com',         get_truncated_week_PRG_time() + INTERVAL '9days 10hours', get_truncated_week_PRG_time() + INTERVAL '9days 16hours' UNION ALL
    -- reservation:         "britney.johansberg   wednesday   7:30    -   8:30" 
    -- repeating timeslot   "Britney Johansberg" | "Mon", "Tue", "Wed", "Thu", "Fri" | 7:30 - 12:00
    SELECT 'britney.johansberg@yaspeak.com', get_truncated_week_PRG_time() + INTERVAL '9days 8hours 30minutes', get_truncated_week_PRG_time() + INTERVAL '9days 12hours' UNION ALL
    -- reservation:         "britney.johansberg   thursday    9:30    -   10:30" 
    -- repeating timeslot   "Britney Johansberg" | "Mon", "Tue", "Wed", "Thu", "Fri" | 7:30 - 12:00
    SELECT 'britney.johansberg@yaspeak.com',  get_truncated_week_PRG_time() + INTERVAL '10days 10hours 30minutes', get_truncated_week_PRG_time() + INTERVAL '10days 12hours'
)
INSERT INTO app_timeslot(owner_id, from_time, iana_timezone, from_local_year, from_local_month, from_local_day, from_local_hour, from_local_minute, duration_in_minutes)
SELECT u.user_id, from_time, 'Europe/Prague', EXTRACT('Year' FROM from_time), EXTRACT('Month' FROM from_time), EXTRACT('Day' FROM from_time), EXTRACT('Hour' FROM from_time), EXTRACT('Minute' FROM from_time), EXTRACT('Epoch' FROM to_time - from_time) / 60
FROM timeslots t
         INNER JOIN app_user u ON t.timeslot_owner = u.login;

-- User Behaviour
INSERT INTO app_user_behaviour (user_id, connected, disconnected, url)
SELECT user_id, get_truncated_week_PRG_time(), get_truncated_week_PRG_time() + INTERVAL '1 minute', '/'
FROM app_user;

-- Notifications
INSERT INTO app_notification
(               recipient_id,   sender_id,          notification_type_id,   notification_state_id,  created,                                read_date_time,                     json_data              )
SELECT * FROM (
                  -- reviews
                  SELECT  reviewee_id,    reviewer_id,        10,                     1,                      created,                                CAST(null AS timestamptz),          CONCAT('{"type": "new_review", "rate": ',rate,', "comment": "',comment,'"}')
                  FROM app_user_review
                  UNION ALL

                  -- new package reservations
                  SELECT  host_id,        attender_id,        9,                      1,                      created,                                CAST(null AS timestamptz),          CONCAT('{"type": "new_package_reservation", "numberOfLessons": ',r.count,'}')
                  FROM app_reservation_package p1
                           INNER JOIN (
                      SELECT reservation_package_id, count(*) as count
                      FROM app_reservation
                      GROUP BY reservation_package_id
                  ) r ON p1.reservation_package_id = r.reservation_package_id AND r.count > 1
                  UNION ALL

                  -- new reservations
                  SELECT r1.host_id,      r1.attender_id,     8,                      1,                      r1.created,                             CAST(null AS timestamptz),          get_reservation_json(r1.reservation_id, 'new_reservation')
                  FROM app_reservation r1
                           LEFT JOIN app_reservation r2 ON r1.reservation_package_id = r2.reservation_package_id AND r1.reservation_id <> r2.reservation_id
                  WHERE r2.reservation_id IS NULL and r1.source_reservation_id IS NULL
                  UNION ALL

                  -- reservation's reminders (for tutors)
                  SELECT r1.host_id,      r1.attender_id,     6,                      1,                      r1.start_time - INTERVAL '15 min',      CAST(null AS timestamptz),          get_reservation_json(r1.reservation_id, 'reservation_reminder')
                  FROM app_reservation r1
                  WHERE r1.start_time < get_PRG_time()
                  UNION ALL

                  -- reservation's reminders (for studuents)
                  SELECT r1.attender_id,  r1.host_id,         6,                      1,                      r1.start_time - INTERVAL '15 min',      CAST(null AS timestamptz),          get_reservation_json(r1.reservation_id, 'reservation_reminder')
                  FROM app_reservation r1
                  WHERE r1.start_time < get_PRG_time()
                  UNION ALL

                  -- reservation cancellation (for tutor)
                  SELECT r1.host_id,      r1.attender_id,     5,                      1,                      r1.created,                             CAST(null AS timestamptz),         get_reservation_json(r1.reservation_id, 'reservation_cancellation')
                  FROM app_reservation r1
                  WHERE r1.reservation_state_id = 2
                  UNION ALL

                  -- package cancellation (for tutor)
                  SELECT p1.host_id,      p1.attender_id,     5,                      1,                      p1.created,                             CAST(null AS timestamptz),         CONCAT('{"type": "package_cancellation", "numberOfLessons": ',r.count,'}')
                  FROM app_reservation_package p1
                           INNER JOIN (
                      SELECT reservation_package_id, count(*) as count
                      FROM app_reservation
                      WHERE reservation_state_id = 2
                      GROUP BY reservation_package_id
                  ) r ON p1.reservation_package_id = r.reservation_package_id AND r.count > 1
                  UNION ALL

                  -- reservation reschedule (student rescheduled)
                  SELECT r1.host_id,      r1.attender_id,     7,                      1,                      r1.created,                             CAST(null AS timestamptz),         get_reservation_json(r1.reservation_id, 'reservation_reschedule')
                  FROM app_reservation r1
                  WHERE r1.source_reservation_id IS NOT NULL
                  UNION ALL

                  -- reservation issues was created (for tutor)
                  SELECT r1.host_id,      r1.attender_id,     12,                     1,                      i.created,                              CAST(null AS timestamptz),          get_reservation_issue_json(i.reservation_issue_id, 'refund_requested', r1.host_id)
                  FROM app_reservation_issue i
                           INNER JOIN app_reservation r1 ON i.reservation_id = r1.reservation_id
                  UNION ALL

                  -- reservation issues was approved (for tutor)
                  SELECT r1.host_id,      1,                  13,                     1,                      i.created,                              CAST(null AS timestamptz),          get_reservation_issue_json(i.reservation_issue_id, 'refund_approved', r1.host_id)
                  FROM app_reservation_issue i
                           INNER JOIN app_reservation r1 ON i.reservation_id = r1.reservation_id
                  WHERE i.reservation_issue_state_id = 2
                  UNION ALL

                  -- reservation issues was approved (for student)
                  SELECT r1.attender_id,  1,                  13,                     1,                      i.created,                              CAST(null AS timestamptz),          get_reservation_issue_json(i.reservation_issue_id, 'refund_declined', r1.attender_id)
                  FROM app_reservation_issue i
                           INNER JOIN app_reservation r1 ON i.reservation_id = r1.reservation_id
                  WHERE i.reservation_issue_state_id = 2
                  UNION ALL

                  -- reservation issues was declined (for tutor)
                  SELECT r1.host_id,      1,                  14,                     1,                      i.resolved,                             CAST(null AS timestamptz),          get_reservation_issue_json(i.reservation_issue_id, 'refund_declined', r1.host_id)
                  FROM app_reservation_issue i
                           INNER JOIN app_reservation r1 ON i.reservation_id = r1.reservation_id
                  WHERE i.reservation_issue_state_id = 3
                  UNION ALL

                  -- reservation issues was declined (for student)
                  SELECT r1.attender_id,  1,                  14,                     1,                      i.resolved,                             CAST(null AS timestamptz),          get_reservation_issue_json(i.reservation_issue_id, 'refund_declined', r1.attender_id)
                  FROM app_reservation_issue i
                           INNER JOIN app_reservation r1 ON i.reservation_id = r1.reservation_id
                  WHERE i.reservation_issue_state_id = 3
                  UNION ALL

                  -- withdrawal requests
                  SELECT creator_id,      1,                  15,                     1,                      t.created,                             CAST(null AS timestamptz),          '{"type": "withdrawal_approved"}'
                  FROM app_transaction t
                  WHERE transaction_type_id = 4
              ) x
ORDER BY created;