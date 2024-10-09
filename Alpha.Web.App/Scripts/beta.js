$('[data-toggle="collapse"]').on('click', function () {
    var $this = $(this),
        $parent = typeof $this.data('parent') !== 'undefined' ? $($this.data('parent')) : undefined;
    if ($parent === undefined) { /* Just toggle my  */
        $this.find('.fa').toggleClass('fa-minus fa-plus');
        return true;
    }

    /* Open element will be close if parent !== undefined */
    var currentIcon = $this.find('.fa');
    currentIcon.toggleClass('fa-minus fa-plus');
    $parent.find('.fa').not(currentIcon).removeClass('fa-plus').addClass('fa-minus');

});
